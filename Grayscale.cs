namespace Recognizer;

public static class GrayscaleTask
{
    public static double[,] ToGrayscale(Pixel[,] original)
    {
        var width = original.GetLength(0);
        var height = original.GetLength(1);
        var grayscale = new double[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var pixel = original[x, y];
                grayscale[x, y] = (0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B) / 255.0;
            }
        }

        return grayscale;
    }
}
