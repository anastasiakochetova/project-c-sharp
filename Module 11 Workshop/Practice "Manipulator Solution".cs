using System;
using NUnit.Framework;
using static System.Math;
using static Manipulation.Manipulator;

namespace Manipulation;

public static class ManipulatorTask
{
	public static double[] MoveManipulatorTo(double x, double y, double alpha)
	{
		ComputeWristCoordinates(x, y, alpha, out var wristX, out var wristY);
		var distToWrist = CalculateDistanceToWrist(wristX, wristY);
		var baseAngle = Atan2(wristY, wristX);
		var shoulder = ComputeShoulderAngle(distToWrist, baseAngle);
		var elbow = ComputeElbowAngle(distToWrist);
		var wrist = ComputeWristAngle(shoulder, elbow, alpha);
		
		if (HasInvalidAngles(shoulder, elbow, wrist))
			return CreateNaNAngles();
		
		return new[] { shoulder, elbow, wrist };
	}

	private static void ComputeWristCoordinates(double x, double y, double alpha,
		out double wristX, out double wristY)
	{
		wristX = x + Cos(PI - alpha) * Palm;
		wristY = y + Sin(PI - alpha) * Palm;
	}

	private static double CalculateDistanceToWrist(double wristX, double wristY)
	{
		return Sqrt(wristX * wristX + wristY * wristY);
	}

	private static double ComputeShoulderAngle(double distToWrist, double baseAngle)
	{
		var triangleAngle = TriangleTask.GetABAngle(UpperArm, distToWrist, Forearm);
		return triangleAngle + baseAngle;
	}

	private static double ComputeElbowAngle(double distToWrist)
	{
		return TriangleTask.GetABAngle(UpperArm, Forearm, distToWrist);
	}

	private static double ComputeWristAngle(double shoulder, double elbow, double alpha)
	{
		return 2 * PI - shoulder - elbow - alpha;
	}

	private static bool HasInvalidAngles(double shoulder, double elbow, double wrist)
	{
		return double.IsNaN(shoulder) || double.IsNaN(elbow) || double.IsNaN(wrist);
	}

	private static double[] CreateNaNAngles()
	{
		return new[] { double.NaN, double.NaN, double.NaN };
	}
}

[TestFixture]
public class ManipulatorTask_Tests
{
	private const double MaxReach = UpperArm + Forearm + Palm;
	private const double Eps = 1e-3;

	[Test]
	public void TestMoveManipulatorTo()
	{
		var rand = new Random();
		const int TestCount = 1111;

		for (int i = 0; i < TestCount; i++)
		{
			var testX = (rand.NextDouble() - 0.5) * MaxReach * 4;
			var testY = (rand.NextDouble() - 0.5) * MaxReach * 4;
			var testAlpha = (rand.NextDouble() - 0.5) * 10;

			var angles = ManipulatorTask.MoveManipulatorTo(testX, testY, testAlpha);

			CheckAngles(angles, testX, testY);
		}
	}

	private static void CheckAngles(double[] angles, double targetX, double targetY)
	{
		if (!double.IsNaN(angles[0]))
		{
			CheckReachable(angles, targetX, targetY);
		}
		else
		{
			CheckUnreachable(angles, targetX, targetY);
		}
	}

	private static void CheckReachable(double[] angles, double targetX, double targetY)
	{
		var points = AnglesToCoordinatesTask.GetJointPositions(
			angles[0],
			angles[1],
			angles[2]);

		Assert.That(points[2].X, Is.EqualTo(targetX).Within(Eps),
			$"X координата не совпадает для достижимой точки ({targetX}, {targetY})");
		Assert.That(points[2].Y, Is.EqualTo(targetY).Within(Eps),
			$"Y координата не совпадает для достижимой точки ({targetX}, {targetY})");
	}

	private static void CheckUnreachable(double[] angles, double targetX, double targetY)
	{
		if (!double.IsNaN(angles[0]) || !double.IsNaN(angles[1]) || !double.IsNaN(angles[2]))
		{
			Assert.Fail($"Все углы должны быть NaN для недостижимой точки ({targetX}, {targetY}).");
		}
	}
}
