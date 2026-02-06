// Вставьте сюда финальное содержимое файла GrayscaleTask.cs
namespace Recognizer;

public static class GrayscaleTask
{
    public static double[,] ToGrayscale(Pixel[,] image)
    {
        int width = image.GetLength(0);
        int height = image.GetLength(1);
        var result = new double[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                result[x, y] = Convert(image[x, y]);

        return result;
    }

    static double Convert(Pixel p) =>
        (0.299 * p.R + 0.587 * p.G + 0.114 * p.B) / 255.0;
}
