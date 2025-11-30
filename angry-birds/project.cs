using System;

public class AngryBirdsTask
{
    public static double FindSightAngle(double velocity, double range)
    {
        const double gravity = 9.8;      
        double projectileParameter = (gravity * range) / (velocity * velocity);       
        if (Math.Abs(projectileParameter) > 1.0)
            return double.NaN;       
        double launchParameter = 0.5 * Math.Asin(projectileParameter);       
        return launchParameter;
    }
}
