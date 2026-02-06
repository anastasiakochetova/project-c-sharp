using System;
using NUnit.Framework;

namespace Manipulation;

public class TriangleTask
{ 
    public static double GetABAngle(double a, double b, double c)
    {
        if (a < 0 || b < 0 || c < 0)
            return double.NaN;
        if (a == 0 || b == 0)
            return double.NaN;
        if (Math.Abs(a + b - c) < 1e-10)
            return Math.PI;
        if (Math.Abs(a - (b + c)) < 1e-10 || Math.Abs(b - (a + c)) < 1e-10)
            return 0;
        if (Math.Abs(c) < 1e-10)
            return 0;
        if (a + b < c || a + c < b || b + c < a)
            return double.NaN;
        double cosAngle = (a * a + b * b - c * c) / (2 * a * b);
        if (cosAngle < -1.0 || cosAngle > 1.0)
            return double.NaN;
        
        return Math.Acos(cosAngle);
    }
}

[TestFixture]
public class TriangleTask_Tests
{
    [TestCase(3, 4, 5, Math.PI / 2, TestName = "Прямоугольный треугольник 3-4-5")]
    [TestCase(1, 1, 1, Math.PI / 3, TestName = "Равносторонний треугольник")]
    [TestCase(150, 120, 60, 0.3897607327974747, TestName = "Большие значения сторон")]
    [TestCase(60, 120, 150, 1.8886200307227774, TestName = "Большие значения сторон (переставлены)")]
    [TestCase(1, 1, 2, Math.PI, TestName = "Вырожденный треугольник (угол 180°)")]
    [TestCase(2, 1, 1, 0, TestName = "Вырожденный треугольник (угол 0°) вариант 1")]
    [TestCase(1, 2, 1, 0, TestName = "Вырожденный треугольник (угол 0°) вариант 2")]
    [TestCase(1, 1, 2.001, double.NaN, TestName = "Нарушение неравенства треугольника (c > a + b)")]
    [TestCase(1, 2.001, 1, double.NaN, TestName = "Нарушение неравенства треугольника (b > a + c)")]
    [TestCase(2.001, 1, 1, double.NaN, TestName = "Нарушение неравенства треугольника (a > b + c)")]
    [TestCase(0, 5, 5, double.NaN, TestName = "Нулевая сторона a")]
    [TestCase(5, 0, 5, double.NaN, TestName = "Нулевая сторона b")]
    [TestCase(5, 5, 0, 0, TestName = "Нулевая сторона c (вырожденный случай)")]
    [TestCase(-3, -2, -4, double.NaN, TestName = "Отрицательные стороны")]
    public void TestGetABAngle(double a, double b, double c, double expectedAngle)
    {
        var actual = TriangleTask.GetABAngle(a, b, c);
        if (double.IsNaN(expectedAngle))
        {
            Assert.That(actual, Is.NaN,
                $"Для сторон треугольника a={a}, b={b}, c={c} ожидался NaN, " +
                $"но получено значение: {actual}");
        }
        else
        {
            Assert.That(actual, Is.EqualTo(expectedAngle).Within(1e-5),
                $"Для сторон треугольника a={a}, b={b}, c={c} " +
                $"ожидался угол {expectedAngle:F6}, но получен {actual:F6}");
        }
    }
}
