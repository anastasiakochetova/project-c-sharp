// Вставьте сюда финальное содержимое файла DistanceTask.cs
using System;

namespace DistanceTask
{
    public static class DistanceTask
    {
        private static double Distance(double x1, double y1, double x2, double y2)
        {
            var dx = x1 - x2;
            var dy = y1 - y2;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double GetDistanceToSegment(
            double ax, double ay,
            double bx, double by,
            double x, double y)
        {
            var abx = bx - ax;
            var aby = by - ay;

            var ahx = x - ax;
            var ahy = y - ay;

            var abLengthSq = abx * abx + aby * aby;

            if (abLengthSq == 0)
                return Distance(x, y, ax, ay);

            var projection = (ahx * abx + ahy * aby) / abLengthSq;

            if (projection < 0)
                return Distance(x, y, ax, ay);

            if (projection > 1)
                return Distance(x, y, bx, by);

            var cx = ax + projection * abx;
            var cy = ay + projection * aby;

            return Distance(x, y, cx, cy);
        }
    }
}
