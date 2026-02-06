namespace Mazes
{
    public static class DiagonalMazeTask
    {
        public static void MoveOut(Robot robot, int width, int height)
        {
            var primary = ChoosePrimary(width, height);
            var secondary = ChooseSecondary(width, height);
            var longRun = ComputeLongRun(width, height);

            while (!robot.Finished)
                MakeStep(robot, longRun, primary, secondary);
        }

        private static Direction ChoosePrimary(int width, int height)
        {
            return width >= height ? Direction.Right : Direction.Down;
        }

        private static Direction ChooseSecondary(int width, int height)
        {
            return width >= height ? Direction.Down : Direction.Right;
        }

        private static int ComputeLongRun(int width, int height)
        {
            var w = width - 2;
            var h = height - 2;
            return width >= height ? w / h : h / w;
        }

        private static void MakeStep(Robot robot, int count, Direction longDir, Direction shortDir)
        {
            MoveStraight(robot, count, longDir);
            if (!robot.Finished)
                robot.MoveTo(shortDir);
        }

        private static void MoveStraight(Robot robot, int steps, Direction dir)
        {
            for (int i = 0; i < steps; i++)
                robot.MoveTo(dir);
        }
    }
}
