// Вставьте сюда финальное содержимое файла DragonFractalTask.cs
using System;

namespace Fractals;

internal static class DragonFractalTask
{
    public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
    {
        var random = new Random(seed);
        double x = 1;
        double y = 0;

        for (int i = 0; i < iterationsCount; i++)
        {
            ApplyRandomTransform(random, ref x, ref y);
            pixels.SetPixel(x, y);
        }
    }

    static void ApplyRandomTransform(Random random, ref double x, ref double y)
    {
        if (random.Next(2) == 0)
            Transform1(ref x, ref y);
        else
            Transform2(ref x, ref y);
    }

    static void Transform1(ref double x, ref double y)
    {
        const double angle = Math.PI / 4;
        double c = Math.Cos(angle);
        double s = Math.Sin(angle);
        double k = Math.Sqrt(2);

        double nx = (x * c - y * s) / k;
        double ny = (x * s + y * c) / k;
        x = nx;
        y = ny;
    }

    static void Transform2(ref double x, ref double y)
    {
        const double angle = 3 * Math.PI / 4;
        double c = Math.Cos(angle);
        double s = Math.Sin(angle);
        double k = Math.Sqrt(2);

        double nx = (x * c - y * s) / k + 1;
        double ny = (x * s + y * c) / k;
        x = nx;
        y = ny;
    }
}
