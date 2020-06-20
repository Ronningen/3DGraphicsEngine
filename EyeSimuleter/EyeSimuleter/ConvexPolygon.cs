using System;
using System.Drawing;
using static System.Math;

namespace EyeSimuleter
{
    /// <summary>
    /// Представляет элементарный видимый объект - плоский выпуклый многоугольник.
    /// </summary>
    class ConvexPolygon
    {
        public Brush colorFill;
        private DirectCoordinate[] vertexes;
        private DirectCoordinate normal;
        /// <summary>
        /// Возвращает одну из вершин многоугольника, исключая ошибку выхода из размера массива.
        /// </summary>
        /// <param name="i"> индексатор. используется только остаток от деления на количество вершин многоугольника. </param>
        /// <returns></returns>
        public DirectCoordinate this[uint i]
        {
            get { return vertexes[i % vertexes.Length]; }
            set { vertexes[i % vertexes.Length] = value; }
        }

        #region constructors

        /// <summary></summary>
        /// <param name="colorFill"> Отображаемая текстура полигона. </param>
        /// <param name="vertexes"> Набор координат вершин полигона. </param>
        public ConvexPolygon(Brush colorFill, DirectCoordinate[] vertexes)
        {
            //проверка, является ли массив точек полигоном:
            if (vertexes.Length < 3)
                throw new Exception("Задан не многоугольник: вершин меньше трех.");

            this.colorFill = colorFill;
            this.vertexes = vertexes;
            //нормаль к плоскости многоугольника задается как векторное произведение его двух первых сторон-векторов.Ы
            normal = (vertexes[1] - vertexes[0]).VectorMultiplicate(vertexes[2] - vertexes[1]);

            //проверка на компланарность всех векторов:
            //  копмланарность определяется относительно первых двух векторов - первые две точки не учитываются.
            //  если все вектора, кроме последнего, компаланрны, то компалнарен и последний - последняя точка не учитывается.
            for (uint i = 2; i < vertexes.Length - 1; i++)
                if (normal.ScalarMultiplication(vertexes[i] - vertexes[i + 1]) != 0)
                    throw new Exception("Многоугольник не плоский.");

            //проверка на выпуклость:
            //  у выпуклого многоугольника все векторные произведения ближайших векторов-сторон направленны в одну сторону.
            //  за шаблон берется нормаль, рассчитанную по двум первым сторонам.
            //  направления векторов сравниваются через знак из скалярного произведения.
            for (uint i = 2; i < vertexes.Length; i++)
                if (normal.ScalarMultiplication((vertexes[i] - vertexes[i - 1]).VectorMultiplicate(this[i + 1] - vertexes[i])) < 0)
                    throw new Exception("Многоугольник не выпуклый.");
        }

        /// <summary>
        /// Создает правильны многоуголник.
        /// </summary>
        /// <param name="colorFill"> Отображаемая текстура полигона. </param>
        /// <param name="angleAmunt"> Количество углов правильного многоугоьника. </param>
        /// <param name="centrePoint"> Координаты центра многоугольника. </param>
        /// <param name="radius"> Задает одну из вершин многоугольника вектором от центра. </param>
        /// <param name="normal"> Координаты вектора нормали к плоскости многоугольника. Ну или хотя бы указывает плоскость нормали. </param>
        public ConvexPolygon(Brush colorFill, int angleAmunt, DirectCoordinate centrePoint, DirectCoordinate radius, DirectCoordinate normal)
        {
            if (angleAmunt < 3)
                angleAmunt = 3;
            this.colorFill = colorFill;

            //новый плоский базис для удобного рассчета вершин:
            DirectCoordinate right = radius.VectorMultiplicate(normal);
            right *= radius.Length / right.Length;

            //проверка на коллинеарность вектора радиуса и нормали:
            this.normal = radius.VectorMultiplicate(right);
            if (this.normal.Length == 0)
                throw new Exception("Нормаль должна быть перпендикулярна плоскости.");

            vertexes = new DirectCoordinate[angleAmunt];

            //рассчет вершин правильного многоугольника:
            double deltaAngle = PI * 2 / angleAmunt;
            for (int i = 0; i < angleAmunt; i++)
                vertexes[i] = centrePoint + (float)Cos(deltaAngle * i) * radius + (float)Sin(deltaAngle * i) * right;
        }

        #endregion

        /// <summary>
        /// Возвращает точку пересечения плоскости полигона и прямой, заданной аргументами.
        /// </summary>
        /// <param name="point1"> первая точка прямой, базовая. </param>
        /// <param name="point2"> вторая точка прямой, направляющая. </param>
        /// <param name="rayCoordinate"> отношение длины вектора от первой точки к точке пересечения и длины направляющего вектора с учетом направления. </param>
        /// <returns> Точка пересечения прямой и многоугольника, если они пересекаются. </returns>
        public DirectCoordinate? GetIntersection(DirectCoordinate point1, DirectCoordinate point2, out float rayCoordinate)
        {
            DirectCoordinate direction = point2 - point1; //направляющий вектор.
            //введение скалярного произведения нормали и напрвляющего вектора для проверки на параллельность:
            float denominator = normal.ScalarMultiplication(direction);
            if (denominator != 0)
            {
                //рассчет отношения длин направляющего вектора и вектора до точки персечения с учетом сонаправленности:
                //  по теореме Фалеса искомое отношение равно отношению проекций расмматриваемых векторов на перпендикуляр к плоскости многоугольника,
                //  а благодаря косинусам отношение проекций равно отношению скалярных произведений на нормаль направляющего вектора и вектора до любой вершины многоугольника.
                rayCoordinate = normal.ScalarMultiplication(vertexes[0] - point1) / denominator;
                //нахождение пересечения прямой и плоскости многоугольника:
                DirectCoordinate intersection = point1 + rayCoordinate * direction;
                //проверка на принадлежность точки пересечения многоугольнику:
                if (this.Contains(intersection))
                    return intersection;
            }
            rayCoordinate = 0;
            return null;
        }

        /// <summary>
        /// Определяет, содержит ли полигон точку.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(DirectCoordinate point)
        {
            //точка внутри полигона, если находится с одной стороны от каждого вектора-стороны выпуклого многоугольника;
            //сторона, с которой лежит точка от вектора-стороны, определяется направлением результата векторного произведения:
            //  направление результата векторного произведения определяется знаком его скалярного произведения с заданной нормалью.
            for (uint i = 0; i < vertexes.Length; i++)
                if (normal.ScalarMultiplication((this[i + 1] - vertexes[i]).VectorMultiplicate(point - this[i + 1])) < 0)
                    return false;
            return true;
        }
    }
}
