using System;

namespace Recognizer;
internal static class SobelFilterTask
{
    public static double[,] SobelFilter(double[,] g, double[,] sx)
    {
        var width = g.GetLength(0);
        var height = g.GetLength(1);
        var result = new double[width, height];

        var kernelSize = sx.GetLength(0);
        var radius = kernelSize / 2;

        ApplySobel(g, sx, result, width, height, kernelSize, radius);

        return result;
    }

    private static void ApplySobel(double[,] g, double[,] sx, double[,] result, int width, int height, int kernelSize, int radius)
    {
        for (var x = radius; x < width - radius; x++)
        {
            for (var y = radius; y < height - radius; y++)
            {
                var gradientX = 0.0;
                var gradientY = 0.0;

                for (var i = 0; i < kernelSize; i++)
                {
                    for (var j = 0; j < kernelSize; j++)
                    {
                        var imgX = x + (i - radius);
                        var imgY = y + (j - radius);

                        gradientX += sx[i, j] * g[imgX, imgY];
                        gradientY += sx[j, i] * g[imgX, imgY];
                    }
                }

                result[x, y] = Math.Sqrt(gradientX * gradientX + gradientY * gradientY);
            }
        }
    }
}
