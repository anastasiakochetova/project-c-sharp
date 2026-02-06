using System;
using System.Drawing;

namespace RoutePlanning
{
    public static class PathFinderTask
    {
        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            var bestLength = double.PositiveInfinity;

            var currentPath = new int[checkpoints.Length];
            var optimalPath = new int[checkpoints.Length];

            currentPath[0] = 0;

            Search(
                checkpoints,
                currentPath,
                1,
                ref bestLength,
                ref optimalPath
            );

            return optimalPath;
        }

        private static void Search(
            Point[] points,
            int[] route,
            int depth,
            ref double bestDistance,
            ref int[] bestRoute)
        {
            var prefix = new int[depth];
            Array.Copy(route, prefix, depth);

            var lengthSoFar = PointExtensions.GetPathLength(points, prefix);

            // отсечение перебора
            if (lengthSoFar >= bestDistance)
                return;

            if (depth == route.Length)
            {
                bestDistance = lengthSoFar;
                bestRoute = (int[])route.Clone();
                return;
            }

            for (int next = 1; next < route.Length; next++)
            {
                if (AlreadyUsed(route, next, depth))
                    continue;

                route[depth] = next;
                Search(points, route, depth + 1, ref bestDistance, ref bestRoute);
            }
        }

        private static bool AlreadyUsed(int[] route, int value, int count)
        {
            for (int i = 0; i < count; i++)
                if (route[i] == value)
                    return true;

            return false;
        }
    }
}
