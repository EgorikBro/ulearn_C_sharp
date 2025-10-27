using System;

namespace DistanceTask;

public static class DistanceTask
{
    public static double GetLength(double ax, double ay, double bx, double by)
    {
        return Math.Sqrt((bx - ax) * (bx - ax) + (by - ay) * (by - ay));
    }

    public static double GetArea(double ab, double bc, double ac)
    {
        var p = (ab + bc +  ac) / 2;
        return Math.Sqrt(p * (p - ab) * (p - bc) * (p - ac));
    }

    // Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
    public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
    {
        var ab = GetLength(ax, ay, bx, by);
        var bc = GetLength(bx, by, x, y);
        var ac = GetLength(ax, ay, x, y);
        var s = GetArea(ab, bc, ac);
        if ((ab * ab + bc * bc >= ac * ac) && (ab * ab + ac * ac >= bc * bc) && ab != 0)
            return 2 * s / ab;
        else 
            return Math.Min(bc, ac);
    }
}
