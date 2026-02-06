using System;
using System.Linq;

public static class ThresholdFilterTask
{
    public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
    {
        int rows = original.GetLength(0);
        int cols = original.GetLength(1);

        double[] pixels = Flatten(original);
        double threshold = ComputeThreshold(pixels, whitePixelsFraction);

        return ApplyThreshold(original, threshold);
    }

    private static double[] Flatten(double[,] image)
    {
        int rows = image.GetLength(0);
        int cols = image.GetLength(1);
        double[] flat = new double[rows * cols];
        int k = 0;
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                flat[k++] = image[i, j];
        return flat;
    }

    private static double ComputeThreshold(double[] pixels, double whitePixelsFraction)
    {
        int total = pixels.Length;
        int minWhite = (int)(whitePixelsFraction * total);
        Array.Sort(pixels);

        if (minWhite == 0)
            return pixels[total - 1] + 1.0; 
        return pixels[total - minWhite];
    }

    private static double[,] ApplyThreshold(double[,] image, double threshold)
    {
        int rows = image.GetLength(0);
        int cols = image.GetLength(1);
        double[,] result = new double[rows, cols];

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                result[i, j] = image[i, j] >= threshold ? 1.0 : 0.0;

        return result;
    }
}
