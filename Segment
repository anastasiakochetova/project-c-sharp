using System;

namespace Geometry
{
    public class Vector
    {
        public double X;
        public double Y;
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;
    }

    public static class Geometry
    {
        private const double Epsilon = 1e-9;

        public static double GetLength(Vector v) =>
            Math.Sqrt(v.X * v.X + v.Y * v.Y);

        public static Vector Add(Vector a, Vector b) =>
            new Vector { X = a.X + b.X, Y = a.Y + b.Y };

        public static double GetLength(Segment s) =>
            GetLength(Subtract(s.End, s.Begin));

        public static bool IsVectorInSegment(Vector point, Segment segment) =>
            IsDegenerate(segment)
                ? AreEqual(point, segment.Begin)
                : IsCollinear(point, segment) && IsBetween(point, segment);

        private static Vector Subtract(Vector a, Vector b) =>
            new Vector { X = a.X - b.X, Y = a.Y - b.Y };

        private static bool IsDegenerate(Segment s) =>
            GetLength(Subtract(s.End, s.Begin)) < Epsilon;

        private static bool AreEqual(Vector a, Vector b) =>
            Math.Abs(a.X - b.X) < Epsilon && Math.Abs(a.Y - b.Y) < Epsilon;

        private static bool IsCollinear(Vector p, Segment s)
        {
            var ab = Subtract(s.End, s.Begin);
            var ap = Subtract(p, s.Begin);
            return Math.Abs(ab.X * ap.Y - ab.Y * ap.X) < Epsilon;
        }

        private static bool IsBetween(Vector p, Segment s)
        {
            var ab = Subtract(s.End, s.Begin);
            var ap = Subtract(p, s.Begin);
            var dot = ap.X * ab.X + ap.Y * ab.Y;
            return dot >= 0 && dot <= ab.X * ab.X + ab.Y * ab.Y;
        }
    }
}
