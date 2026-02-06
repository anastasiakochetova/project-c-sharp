using System;

public class RectanglesTask
{
    // Проверяем, пересекаются ли прямоугольники хотя бы в одной точке
    public static bool AreIntersected(Rectangles.Rectangle a, Rectangles.Rectangle b)
    {
        bool overlapX = a.Left <= b.Right && b.Left <= a.Right;   // пересечение по X
        bool overlapY = a.Top <= b.Bottom && b.Top <= a.Bottom;   // пересечение по Y
        return overlapX && overlapY;
    }

    // Считаем площадь пересечения. Если нет пересечения — вернётся 0
    public static int IntersectionSquare(Rectangles.Rectangle a, Rectangles.Rectangle b)
    {
        if (!AreIntersected(a, b))
            return 0;

        int left = Math.Max(a.Left, b.Left);       // левая граница пересечения
        int top = Math.Max(a.Top, b.Top);          // верхняя граница
        int right = Math.Min(a.Right, b.Right);    // правая граница
        int bottom = Math.Min(a.Bottom, b.Bottom); // нижняя граница

        int width = right - left;
        int height = bottom - top;

        if (width < 0 || height < 0)
            return 0;

        return width * height;
    }

    // Проверяем, полностью ли один прямоугольник лежит внутри другого
    public static bool IsRectangleInside(Rectangles.Rectangle inner, Rectangles.Rectangle outer)
    {
        bool insideX = inner.Left >= outer.Left && inner.Right <= outer.Right;
        bool insideY = inner.Top >= outer.Top && inner.Bottom <= outer.Bottom;
        return insideX && insideY;
    }

    // Определяем, какой из прямоугольников вложен в другой
    // Если первый лежит внутри второго — возвращаем 0, если наоборот — 1
    // Если ни один не вложен — возвращаем -1
    public static int IndexOfInnerRectangle(Rectangles.Rectangle r1, Rectangles.Rectangle r2)
    {
        if (IsRectangleInside(r1, r2))
            return 0;
        if (IsRectangleInside(r2, r1))
            return 1;
        return -1;
    }
}
