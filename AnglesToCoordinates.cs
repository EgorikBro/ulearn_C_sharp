using System;
using Avalonia;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using static Manipulation.Manipulator;

namespace Manipulation;

public static class AnglesToCoordinatesTask
{
	/// <summary>
	/// По значению углов суставов возвращает массив координат суставов
	/// в порядке new []{elbow, wrist, palmEnd}
	/// </summary>
	public static Point[] GetJointPositions(double shoulder, double elbow, double wrist)
	{
		var elbowPos = new Point(UpperArm * Math.Cos(shoulder), UpperArm * Math.Sin(shoulder));
		
		var forearmAngle = shoulder + Math.PI + elbow;
		var wristPos = new Point(
			elbowPos.X + Forearm * Math.Cos(forearmAngle),
			elbowPos.Y + Forearm * Math.Sin(forearmAngle));

		var palmAngle = forearmAngle + Math.PI + wrist;
		var palmEndPos = new Point(
			wristPos.X + Palm * Math.Cos(palmAngle),
			wristPos.Y + Palm * Math.Sin(palmAngle));
		
		return new[]
		{
			elbowPos,
			wristPos,
			palmEndPos
		};
	}
}

[TestFixture]
public class AnglesToCoordinatesTask_Tests
{
	[TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Forearm + Palm, UpperArm)]
	[TestCase(0, Math.PI, Math.PI, UpperArm + Forearm + Palm, 0)]
	[TestCase(Math.PI / 2, Math.PI, Math.PI, 0, UpperArm + Forearm + Palm)]
	[TestCase(Math.PI / 2, 0, 0, 0, UpperArm - Forearm + Palm)]
	public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
	{
		var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
		ClassicAssert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
		ClassicAssert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
		
		ClassicAssert.AreEqual(UpperArm, Distance(new Point(0, 0), joints[0]), 1e-5, "UpperArm length");
		ClassicAssert.AreEqual(Forearm, Distance(joints[0], joints[1]), 1e-5, "Forearm length");
		ClassicAssert.AreEqual(Palm, Distance(joints[1], joints[2]), 1e-5, "Palm length");
	}

	private double Distance(Point p1, Point p2)
	{
		return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
	}
}
