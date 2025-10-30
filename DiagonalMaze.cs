namespace Mazes
{
    public static class DiagonalMazeTask
    {
        public static void MoveOut(Robot robot, int width, int height)
        {
            var numberSteps = CalculateNumberSteps(width, height);
            var (firstDirection, secondDirection) = GetDirections(width, height);

            while (!robot.Finished)
                Move(robot, numberSteps, firstDirection, secondDirection);
        }

        private static (Direction first, Direction second) GetDirections(int width, int height)
        {
            return height > width 
                ? (Direction.Down, Direction.Right) 
                : (Direction.Right, Direction.Down);
        }

        private static int CalculateNumberSteps(int width, int height)
        {
            return height > width
                ? (height - 2) / (width - 2)
                : (width - 2) / (height - 2);
        }

        private static void Move(Robot robot, int stepCount, Direction longDirection, Direction shortDirection)
        {
            for (var i = 0; i < stepCount; i++)
                robot.MoveTo(longDirection);

            if (!robot.Finished)
                robot.MoveTo(shortDirection);
        }
    }
}
