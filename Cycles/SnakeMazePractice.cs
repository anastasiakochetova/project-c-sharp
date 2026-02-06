namespace Mazes
{
    public static class SnakeMazeTask
    {
        public static void MoveOut(Robot robot, int width, int height)
        {
            var horizontal = width - 3;
            var vertical = 2;

            while (!robot.Finished)
            {
                MakeSnakeStep(robot, horizontal, Direction.Right, vertical);
            }
        }

        private static void MakeSnakeStep(Robot robot, int horizontal, Direction firstDirection, int vertical)
        {
            MoveHorizontal(robot, horizontal, firstDirection);
            MoveDown(robot, vertical);

            if (robot.Finished)
                return;

            MoveHorizontal(robot, horizontal, ReverseDirection(firstDirection));
            MoveDown(robot, vertical);
        }

        private static void MoveHorizontal(Robot robot, int steps, Direction direction)
        {
            MoveSeveral(robot, steps, direction);
        }

        private static void MoveDown(Robot robot, int steps)
        {
            MoveSeveral(robot, steps, Direction.Down);
        }

        private static void MoveSeveral(Robot robot, int count, Direction direction)
        {
            for (int i = 0; i < count && !robot.Finished; i++)
                robot.MoveTo(direction);
        }

        private static Direction ReverseDirection(Direction dir)
        {
            return dir == Direction.Left ? Direction.Right : Direction.Left;
        }
    }
}
