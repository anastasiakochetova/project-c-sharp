// Вставьте сюда финальное содержимое файла VisualizerTask.cs

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;

namespace Manipulation;

public static class VisualizerTask
{
	public static double X = 220;
	public static double Y = -100;
	public static double Alpha = 0.05;
	public static double Wrist = 2 * Math.PI / 3;
	public static double Elbow = 3 * Math.PI / 4;
	public static double Shoulder = Math.PI / 2;

	public static Brush UnreachableAreaBrush = new SolidColorBrush(Color.FromArgb(255, 255, 230, 230));
	public static Brush ReachableAreaBrush = new SolidColorBrush(Color.FromArgb(255, 230, 255, 230));
	public static Pen ManipulatorPen = new Pen(Brushes.Black, 3);
	public static Brush JointBrush = new SolidColorBrush(Colors.Gray);

	public static void KeyDown(Visual visual, KeyEventArgs key)
	{
		const double angleStep = 0.05; // небольшая величина изменения угла

		if (TryHandleShoulderKey(key, angleStep))
		{
			UpdateWristAngle();
		}
		else if (TryHandleElbowKey(key, angleStep))
		{
			UpdateWristAngle();
		}

		visual.InvalidateVisual(); // вызывает перерисовку канваса
	}

	private static bool TryHandleShoulderKey(KeyEventArgs key, double angleStep)
	{
		switch (key.Key)
		{
			case Key.Q:
				Shoulder += angleStep;
				return true;
			case Key.A:
				Shoulder -= angleStep;
				return true;
			default:
				return false;
		}
	}

	private static bool TryHandleElbowKey(KeyEventArgs key, double angleStep)
	{
		switch (key.Key)
		{
			case Key.W:
				Elbow += angleStep;
				return true;
			case Key.S:
				Elbow -= angleStep;
				return true;
			default:
				return false;
		}
	}

	private static void UpdateWristAngle()
	{
		Wrist = -Alpha - Shoulder - Elbow;
	}

	public static void MouseMove(Visual visual, PointerEventArgs e)
	{
		var shoulderPos = GetShoulderPos(visual);
		var windowPoint = e.GetPosition(visual);
		var mathPoint = ConvertWindowToMath(windowPoint, shoulderPos);
		X = mathPoint.X;
		Y = mathPoint.Y;

		UpdateManipulator();
		visual.InvalidateVisual();
	}

	public static void MouseWheel(Visual visual, PointerWheelEventArgs e)
	{
		const double alphaStep = 0.05;
		Alpha += e.Delta.Y * alphaStep;

		UpdateManipulator();
		visual.InvalidateVisual();
	}

	public static void UpdateManipulator()
	{
		var angles = ManipulatorTask.MoveManipulatorTo(X, Y, Alpha);
		if (!double.IsNaN(angles[0]))
			Shoulder = angles[0];
		if (!double.IsNaN(angles[1]))
			Elbow = angles[1];
		if (!double.IsNaN(angles[2]))
			Wrist = angles[2];
	}

	public static void DrawManipulator(DrawingContext context, Point shoulderPos)
	{
		var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);

		DrawReachableZone(context, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos, joints);
		DrawCoordinatesInfo(context);
		DrawManipulatorSegments(context, shoulderPos, joints);
		DrawManipulatorJoints(context, shoulderPos, joints);
	}

	private static void DrawCoordinatesInfo(DrawingContext context)
	{
		var formattedText = new FormattedText(
			$"X={X:0}, Y={Y:0}, Alpha={Alpha:0.00}",
			CultureInfo.InvariantCulture,
			FlowDirection.LeftToRight,
			Typeface.Default,
			18,
			Brushes.DarkRed
		)
		{
			TextAlignment = TextAlignment.Center
		};
		context.DrawText(formattedText, new Point(10, 10));
	}

	private static void DrawManipulatorSegments(DrawingContext context, Point shoulderPos, Point[] joints)
	{
		// Преобразуем логические координаты в оконные
		var shoulderWindow = ConvertMathToWindow(new Point(0, 0), shoulderPos);
		var elbowWindow = ConvertMathToWindow(joints[0], shoulderPos);
		var wristWindow = ConvertMathToWindow(joints[1], shoulderPos);
		var palmEndWindow = ConvertMathToWindow(joints[2], shoulderPos);

		// Рисуем сегменты манипулятора
		DrawSegment(context, shoulderWindow, elbowWindow);
		DrawSegment(context, elbowWindow, wristWindow);
		DrawSegment(context, wristWindow, palmEndWindow);
	}

	private static void DrawSegment(DrawingContext context, Point start, Point end)
	{
		context.DrawLine(ManipulatorPen, start, end);
	}

	private static void DrawManipulatorJoints(DrawingContext context, Point shoulderPos, Point[] joints)
	{
		const double jointRadius = 8;

		// Преобразуем логические координаты в оконные
		var shoulderWindow = ConvertMathToWindow(new Point(0, 0), shoulderPos);
		var elbowWindow = ConvertMathToWindow(joints[0], shoulderPos);
		var wristWindow = ConvertMathToWindow(joints[1], shoulderPos);
		var palmEndWindow = ConvertMathToWindow(joints[2], shoulderPos);

		// Рисуем суставы
		DrawJoint(context, shoulderWindow, jointRadius);
		DrawJoint(context, elbowWindow, jointRadius);
		DrawJoint(context, wristWindow, jointRadius);
		DrawJoint(context, palmEndWindow, jointRadius);
	}

	private static void DrawJoint(DrawingContext context, Point center, double radius)
	{
		context.DrawEllipse(JointBrush, null, center, radius, radius);
	}

	private static void DrawReachableZone(
		DrawingContext context,
		Brush reachableBrush,
		Brush unreachableBrush,
		Point shoulderPos,
		Point[] joints)
	{
		var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
		var rmax = Manipulator.UpperArm + Manipulator.Forearm;
		var mathCenter = new Point(joints[2].X - joints[1].X, joints[2].Y - joints[1].Y);
		var windowCenter = ConvertMathToWindow(mathCenter, shoulderPos);
		context.DrawEllipse(reachableBrush,
			null,
			new Point(windowCenter.X, windowCenter.Y),
			rmax, rmax);
		context.DrawEllipse(unreachableBrush,
			null,
			new Point(windowCenter.X, windowCenter.Y),
			rmin, rmin);
	}

	public static Point GetShoulderPos(Visual visual)
	{
		return new Point(visual.Bounds.Width / 2, visual.Bounds.Height / 2);
	}

	public static Point ConvertMathToWindow(Point mathPoint, Point shoulderPos)
	{
		return new Point(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);
	}

	public static Point ConvertWindowToMath(Point windowPoint, Point shoulderPos)
	{
		return new Point(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
	}
}
