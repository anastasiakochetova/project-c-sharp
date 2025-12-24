using System;

namespace Geometry
{
    public class Vector
    {
        public double X;
        public double Y;
    }

    public class Geometry
    {
        public static double GetLength(Vector v)
        {
            double dx = v.X;
            double dy = v.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static Vector Add(Vector left, Vector right)
        {
            Vector result = new Vector();
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            return result;
        }
    }
}
