using System;
namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] input, double[,] kernelX)
        {
            int rows = input.GetLength(0);
            int cols = input.GetLength(1);
            double[,] output = new double[rows, cols];

            double[,] kernelY = Transpose(kernelX);
            int offset = kernelX.GetLength(0) / 2;

            for (int i = offset; i < rows - offset; i++)
            {
                for (int j = offset; j < cols - offset; j++)
                {
                    double gx = ConvolveAt(input, kernelX, i, j, offset);
                    double gy = ConvolveAt(input, kernelY, i, j, offset);
                    output[i, j] = Math.Sqrt(gx * gx + gy * gy);
                }
            }

            return output;
        }

        private static double[,] Transpose(double[,] matrix)
        {
            int r = matrix.GetLength(0);
            int c = matrix.GetLength(1);
            double[,] result = new double[c, r];
            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++)
                    result[j, i] = matrix[i, j];
            return result;
        }

        private static double ConvolveAt(double[,] image, double[,] kernel, int x, int y, int offset)
        {
            double sum = 0.0;
            int kRows = kernel.GetLength(0);
            int kCols = kernel.GetLength(1);

            for (int i = 0; i < kRows; i++)
                for (int j = 0; j < kCols; j++)
                    sum += kernel[i, j] * image[x + i - offset, y + j - offset];

            return sum;
        }
    }
}
