using System;

public class BilliardsTask
{
    public static double BounceWall(double startAngle, double wallDirection)
    {
        double angleDifference = startAngle - wallDirection;
        double reflectedAngle = wallDirection - angleDifference;
        reflectedAngle = NormalizeAngle(reflectedAngle);
        
        return reflectedAngle;
    }
    
    private static double NormalizeAngle(double angle)
    {
        double normalized = angle;
        const double fullCircle = 2 * Math.PI;
        while (normalized < 0)
        {
            normalized += fullCircle;
        }
        while (normalized >= fullCircle)
        {
            normalized -= fullCircle;
        }
        return normalized;
    }
}
