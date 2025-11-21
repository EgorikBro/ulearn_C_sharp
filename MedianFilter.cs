using System.Collections.Generic;

namespace Recognizer;

internal static class MedianFilterTask
{
    public static double[,] MedianFilter(double[,] original)
    {
        var width = original.GetLength(0);
        var height = original.GetLength(1);
        var filtered = new double[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var values = GetNeighborhoodValues(original, x, y);
                values.Sort();
                var count = values.Count;
                if (count % 2 == 1)
                    filtered[x, y] = values[count / 2];
                else
                    filtered[x, y] = (values[count / 2 - 1] + values[count / 2]) / 2.0;
            }
        }
        return filtered;
    }

    private static List<double> GetNeighborhoodValues(double[,] original, int x, int y)
    {
        var width = original.GetLength(0);
        var height = original.GetLength(1);
        var values = new List<double>();

        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                var nx = x + dx;
                var ny = y + dy;
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    values.Add(original[nx, ny]);
            }
        }

        return values;
    }
}
