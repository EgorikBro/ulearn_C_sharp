using System;

namespace Fractals
{
    internal static class DragonFractalTask
    {
        public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
        {
            var x = 1.0d;
            var y = 0.0d;
            var randomSeed = new Random(seed);
            for (int i = 0; i < iterationsCount; i++)
            {
                var randomNumber = randomSeed.Next(2);
                (x, y) = GetNextPixel(x, y, randomNumber == 1 ? 45 : 135);
                pixels.SetPixel(x, y);
            }
        }

        private static (double, double) GetNextPixel(double x, double y, int angle)
        {
            var x1 = (x * Math.Cos(Math.PI * angle / 180) - y * Math.Sin(Math.PI * angle / 180)) / Math.Sqrt(2);
            var y1 = (x * Math.Sin(Math.PI * angle / 180) + y * Math.Cos(Math.PI * angle / 180)) / Math.Sqrt(2);
            if (angle == 135)
                x1++;
            return (x1, y1);
        }
    }
}
