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
		const double step = Math.PI / 180;
		switch (key.Key)
		{
			case Key.Q:
				Shoulder += step;
				break;
			case Key.A:
				Shoulder -= step;
				break;
			case Key.W:
				Elbow += step;
				break;
			case Key.S:
				Elbow -= step;
				break;
		}

		Wrist = -Alpha - Shoulder - Elbow;

		visual.InvalidateVisual();
	}

	public static void MouseMove(Visual visual, PointerEventArgs e)
	{
		var p = e.GetPosition(visual);
		var shoulderPos = GetShoulderPos(visual);
		var mathP = ConvertWindowToMath(p, shoulderPos);
		
		X = mathP.X;
		Y = mathP.Y;

		UpdateManipulator();
		visual.InvalidateVisual();
	}

	public static void MouseWheel(Visual visual, PointerWheelEventArgs e)
	{
		Alpha += e.Delta.Y * 0.05;

		UpdateManipulator();
		visual.InvalidateVisual();
	}

	public static void UpdateManipulator()
	{
		var angles = ManipulatorTask.MoveManipulatorTo(X, Y, Alpha);
		if (!double.IsNaN(angles[0]) && !double.IsNaN(angles[1]) && !double.IsNaN(angles[2]))
		{
			Shoulder = angles[0];
			Elbow = angles[1];
			Wrist = angles[2];
		}
	}

	public static void DrawManipulator(DrawingContext context, Point shoulderPos)
	{
		var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);

		DrawReachableZone(context, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos, joints);
		DrawManipulatorSegments(context, shoulderPos, joints);
		DrawDebugText(context);
	}

	private static void DrawManipulatorSegments(DrawingContext context, Point shoulderPos, Point[] joints)
	{
		var joint0 = ConvertMathToWindow(joints[0], shoulderPos);
		var joint1 = ConvertMathToWindow(joints[1], shoulderPos);
		var joint2 = ConvertMathToWindow(joints[2], shoulderPos);

		context.DrawLine(ManipulatorPen, shoulderPos, joint0);
		context.DrawLine(ManipulatorPen, joint0, joint1);
		context.DrawLine(ManipulatorPen, joint1, joint2);

		const double r = 5;
		context.DrawEllipse(JointBrush, null, shoulderPos, r, r);
		context.DrawEllipse(JointBrush, null, joint0, r, r);
		context.DrawEllipse(JointBrush, null, joint1, r, r);
		context.DrawEllipse(JointBrush, null, joint2, r, r);
	}

	private static void DrawDebugText(DrawingContext context)
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
