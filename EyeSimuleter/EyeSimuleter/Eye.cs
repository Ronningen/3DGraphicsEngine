using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static System.Math;

using static EyeSimuleter.EyeParams;

namespace EyeSimuleter
{
    class Eye
    {
        private DirectCoordinate location;
        private double hor; //представляет угл поврота в горизонтальной плоскости
        private double vert; //представляет угл поврота по вертикали

        private Point? rotatePoint;
        public (bool forward, bool backward, bool left, bool right, bool up, bool down) moving;

        public Eye()
        {
            location = new DirectCoordinate(0, 0, 0);
            hor = 0;
            vert = 0;
            moving = (false, false, false, false, false, false);
        }

        /// <summary>
        /// Отображает видимые полигоны, расчитывая пересечения для каждого пикселя размером pixelSize.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="decor"> Список полигонов к просмотр. </param>
        public void ShowLookRayPixel(Graphics g, List<ConvexPolygon> decor)
        {
            //Выделение памяти и определение локальных констант:
            List<(float distance, Brush colorFill)> intersections = new List<(float, Brush)>();
            DirectCoordinate lazerSource = location + observingDistance * new DirectCoordinate(hor, vert);
            DirectCoordinate leftFrameVector = new DirectCoordinate(hor + PI / 2, 0);
            DirectCoordinate upFrameVector = new DirectCoordinate(hor, vert + PI / 2);
            DirectCoordinate currentPixel;

            for (uint i = 1; i < width / pixelSize; i++)
                for (uint j = 1; j < width / pixelSize; j++)
                {
                    intersections.Clear();
                    currentPixel = location
                        + (width / 2 - i * pixelSize) * leftFrameVector
                        + (height / 2 - j * pixelSize) * upFrameVector;

                    //Нахождение пересечений луча зрения с полигонами:
                    foreach (ConvexPolygon polygon in decor)
                        if (polygon.GetIntersection(lazerSource, currentPixel, out float rayCordinate) is DirectCoordinate intersection && rayCordinate > 1)
                            intersections.Add(((intersection - lazerSource).Length, polygon.colorFill));

                    //Закраска пикселя в соответсвии с расположением видимых барьеров
                    foreach (Brush brush in intersections.OrderByDescending(p => p.distance).Select(p => p.colorFill))
                        g.FillRectangle(brush, i * pixelSize - pixelSize / 2, j * pixelSize - pixelSize / 2, pixelSize, pixelSize);
                }
        }

        #region movement control

        /// <summary>
        /// Изменяет направление обзора в соответсвии с движением мышки с зажатой лкм. 
        /// Null-аргумент прекращает вращение.
        /// </summary>
        /// <param name="nextPoint"></param>
        public void Rotate(Point? nextPoint)
        {
            if (rotatePoint != null)
            {
                hor += (double)(nextPoint?.X - rotatePoint?.X?? 0) / 100;
                hor %= 2 * PI;
                vert += (double)(nextPoint?.Y - rotatePoint?.Y ?? 0) / 100;
                vert = (vert >= PI / 2) ? (PI / 2 - 0.001) : ((vert <= -PI / 2) ? (-PI / 2 + 0.001) : vert);
            }
            rotatePoint = nextPoint;
        }

        /// <summary>
        /// Передвигает точку обзора на EyeParams.speed в соответсии с заданными направлениями hor и vert.
        /// </summary>
        public void Move()
        {
            if (moving.forward ^ moving.backward)
                location += (moving.forward ? 1 : -1) * speed * new DirectCoordinate(hor, verticalMovingAllowed ? vert : 0);
            if (moving.left ^ moving.right)
                location += (moving.left ? 1 : -1) * speed * new DirectCoordinate((float)Cos(hor - PI / 2), (float)Sin(hor - PI / 2), 0);
            if (moving.up ^ moving.down)
                location += (0, 0, (moving.down ? 1 : -1) * speed);
        }

        #endregion
    }
}
