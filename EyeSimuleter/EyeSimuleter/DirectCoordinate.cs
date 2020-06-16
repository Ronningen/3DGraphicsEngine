using static System.Math;

namespace EyeSimuleter
{
    /// <summary>
    /// Представляет точку/вектор в декартовой трехмерной системе координат.
    /// </summary>
    public struct DirectCoordinate
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Length { get { return (float)Sqrt(Pow(X, 2) + Pow(Y, 2) + Pow(Z, 2)); } }

        public DirectCoordinate(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static implicit operator DirectCoordinate((float x, float y, float z) input)
        {
            return new DirectCoordinate(input.x, input.y, input.z);
        }

        /// <summary>
        /// Создает единичный вектор, ориентированный небесными координатами hor и vert.
        /// </summary>
        /// <param name="hor"> угл поврота в горизонтальной плоскости </param>
        /// <param name="vert"> угл поврота по вертикали </param>
        public DirectCoordinate(double hor, double vert)
        {
            X = (float)(Cos(vert) * Cos(hor));
            Y = (float)(Cos(vert) * Sin(hor));
            Z = (float)Sin(vert);
        }

        /// <summary>
        /// Векторное произведение данного вектора на вектор multiplier. 
        ///     Помните об антикоммунитативности!
        /// </summary>
        /// <param name="multiplier"> Правый вектор произведения. </param>
        /// <returns></returns>
        public DirectCoordinate VectorMultiplicate(DirectCoordinate multiplier)
        {
            return new DirectCoordinate(
                Y * multiplier.Z - Z * multiplier.Y,
                Z * multiplier.X - X * multiplier.Z,
                X * multiplier.Y - Y * multiplier.X);
        }

        public float ScalarMultiplication(DirectCoordinate multiplier)
        {
            return X * multiplier.X + Y * multiplier.Y + Z * multiplier.Z;
        }

        public static DirectCoordinate operator +(DirectCoordinate vector1, DirectCoordinate vector2)
        {
            return new DirectCoordinate(
                vector1.X + vector2.X,
                vector1.Y + vector2.Y,
                vector1.Z + vector2.Z);
        }

        public static DirectCoordinate operator -(DirectCoordinate vector1, DirectCoordinate vector2)
        {
            return new DirectCoordinate(
                vector1.X - vector2.X,
                vector1.Y - vector2.Y,
                vector1.Z - vector2.Z);
        }

        public static DirectCoordinate operator *(float multiplier, DirectCoordinate vector)
        {
            return new DirectCoordinate(
                vector.X * multiplier,
                vector.Y * multiplier,
                vector.Z * multiplier);
        }
    }
}
