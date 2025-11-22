using System.Collections.Generic;

namespace Recognizer;

public static class ThresholdFilterTask
{
    public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
    {
        var threshold = FindThreshold(original, whitePixelsFraction);
        var width = original.GetLength(0);
        var height = original.GetLength(1);
        var result = new double[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                result[x, y] = original[x, y] >= threshold ? 1.0 : 0.0;
            }
        }

        return result;
    }

    private static double FindThreshold(double[,] original, double whitePixelsFraction)
    {
        var width = original.GetLength(0);
        var height = original.GetLength(1);
        var total = width * height;
        var requiredWhite = (int)(whitePixelsFraction * total);

        var values = new List<double>(total);
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				values.Add(original[x, y]);
			}
		}

        values.Sort();
        if (requiredWhite <= 0)
			return double.MaxValue;

        if (requiredWhite >= total)
            return double.MinValue;

        return values[total - requiredWhite];
    }
}
