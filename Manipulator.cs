using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using static Manipulation.Manipulator;

namespace Manipulation;

public static class ManipulatorTask
{
	/// <summary>
	/// Возвращает массив углов (shoulder, elbow, wrist),
	/// необходимых для приведения эффектора манипулятора в точку x и y 
	/// с углом между последним суставом и горизонталью, равному alpha (в радианах)
	/// См. чертеж manipulator.png!
	/// </summary>
	public static double[] MoveManipulatorTo(double x, double y, double alpha)
	{
		var wristX = x - Palm * Math.Cos(alpha);
		var wristY = y + Palm * Math.Sin(alpha);

		var dist = Math.Sqrt(wristX * wristX + wristY * wristY);
		var elbow = TriangleTask.GetABAngle(UpperArm, Forearm, dist);
		var shoulder = TriangleTask.GetABAngle(UpperArm, dist, Forearm) + Math.Atan2(wristY, wristX);
		var wrist = -alpha - shoulder - elbow;

		if (double.IsNaN(elbow) || double.IsNaN(shoulder) || double.IsNaN(wrist))
			return new[] { double.NaN, double.NaN, double.NaN };

		return new[] { shoulder, elbow, wrist };
	}
}

[TestFixture]
public class ManipulatorTask_Tests
{
	[Test]
	public void TestMoveManipulatorTo()
	{
		var random = new Random();
		for (var i = 0; i < 1000; i++)
		{
			var x = (random.NextDouble() - 0.5) * 600;
			var y = (random.NextDouble() - 0.5) * 600;
			var alpha = (random.NextDouble() - 0.5) * 2 * Math.PI;

			VerifyCoordinates(x, y, alpha);
		}
	}

	private void VerifyCoordinates(double x, double y, double alpha)
	{
		var angles = ManipulatorTask.MoveManipulatorTo(x, y, alpha);

		var wristX = x - Palm * Math.Cos(alpha);
		var wristY = y + Palm * Math.Sin(alpha);
		var dist = Math.Sqrt(wristX * wristX + wristY * wristY);
		var minR = Math.Abs(UpperArm - Forearm);
		var maxR = UpperArm + Forearm;

		if (double.IsNaN(angles[0]))
		{
			ClassicAssert.AreEqual(dist < minR || dist > maxR, true, 
				$"MoveManipulatorTo returned NaN for reachable point: x={x}, y={y}, alpha={alpha}, dist={dist}");
		}
		else
		{
			var joints = AnglesToCoordinatesTask.GetJointPositions(angles[0], angles[1], angles[2]);
			var endEffector = joints[2];
			ClassicAssert.AreEqual(endEffector.X, x, 1e-4, "End effector X mismatch");
			ClassicAssert.AreEqual(endEffector.Y, y, 1e-4, "End effector Y mismatch");
		}
	}
}
