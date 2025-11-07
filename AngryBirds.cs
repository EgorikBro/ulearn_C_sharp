using System;

namespace AngryBirds;

public static class AngryBirdsTask
{
    public static double FindSightAngle(double v, double distance)
    {
        return Math.Asin((distance * 9.8) / (v * v)) / 2;
    }
}
