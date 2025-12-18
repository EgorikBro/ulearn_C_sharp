using System;
using NUnit.Framework;

namespace Manipulation;

public class TriangleTask
{
    /// <summary>
    /// Возвращает угол (в радианах) между сторонами a и b в треугольнике со сторонами a, b, c 
    /// </summary>
    public static double GetABAngle(double a, double b, double c)
    {
        if (a < 0 || b < 0 || c < 0) return double.NaN;
        if (a == 0 || b == 0) return double.NaN;
        
        if (a + b < c || a + c < b || b + c < a) return double.NaN;

        var cos = (a * a + b * b - c * c) / (2 * a * b);
        
        if (cos > 1) cos = 1;
        if (cos < -1) cos = -1;
        
        return Math.Acos(cos);
    }
}

[TestFixture]
public class TriangleTask_Tests
{
    [TestCase(3, 4, 5, Math.PI / 2)]
    [TestCase(1, 1, 1, Math.PI / 3)]
    [TestCase(1, 1, 0, 0)]
    [TestCase(0, 1, 1, double.NaN)]
    [TestCase(1, 0, 1, double.NaN)]
    [TestCase(-1, 1, 1, double.NaN)]
    [TestCase(1, 2, 4, double.NaN)]
    [TestCase(1, 1, 2, Math.PI)]
    [TestCase(2, 3, 1, 0)]
    public void TestGetABAngle(double a, double b, double c, double expectedAngle)
    {
        var angle = TriangleTask.GetABAngle(a, b, c);
        if (double.IsNaN(expectedAngle))
        {
            Assert.That(angle, Is.NaN);
        }
        else
        {
            Assert.That(angle, Is.EqualTo(expectedAngle).Within(1e-5));
        }
    }
}
