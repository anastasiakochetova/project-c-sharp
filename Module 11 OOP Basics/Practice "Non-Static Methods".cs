using System;

namespace Geometry
{
    public class Vector
    {
        public double X;
        public double Y;

        public double GetLength() =>
            Geometry.GetLength(this);

        public Vector Add(Vector other) =>
            Geometry.Add(this, other);

        public bool Belongs(Segment segment) =>
            Geometry.IsVectorInSegment(this, segment);
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public double GetLength() =>
            Geometry.GetLength(this);

        public bool Contains(Vector point) =>
            Geometry.IsVectorInSegment(point, this);
    }

    public static class Geometry
    {
        private const double Epsilon = 1e-9;

        public static double GetLength(Vector v) =>
            Math.Sqrt(v.X * v.X + v.Y * v.Y);

        public static Vector Add(Vector a, Vector b) =>
            new Vector { X = a.X + b.X, Y = a.Y + b.Y };

        public static double GetLength(Segment s) =>
            GetLength(new Vector
            {
                X = s.End.X - s.Begin.X,
                Y = s.End.Y - s.Begin.Y
            });

        public static bool IsVectorInSegment(Vector point, Segment segment)
        {
            var ab = new Vector
            {
                X = segment.End.X - segment.Begin.X,
                Y = segment.End.Y - segment.Begin.Y
            };

            var abLenSq = ab.X * ab.X + ab.Y * ab.Y;

            if (abLenSq < Epsilon)
                return AreEqual(point, segment.Begin);

            var ap = new Vector
            {
                X = point.X - segment.Begin.X,
                Y = point.Y - segment.Begin.Y
            };

            var cross = ab.X * ap.Y - ab.Y * ap.X;
            if (Math.Abs(cross) > Epsilon)
                return false;

            var dot = ap.X * ab.X + ap.Y * ab.Y;
            return dot >= 0 && dot <= abLenSq;
        }

        private static bool AreEqual(Vector a, Vector b) =>
            Math.Abs(a.X - b.X) < Epsilon &&
            Math.Abs(a.Y - b.Y) < Epsilon;
    }
}
