using System;
using System.Drawing;

namespace RoutePlanning;

public static class PathFinderTask
{
	public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
	{
		if (checkpoints.Length <= 1)
			return new int[] { 0 };

		var bestOrder = new int[checkpoints.Length];
		var currentOrder = new int[checkpoints.Length];
		currentOrder[0] = 0;

		var used = new bool[checkpoints.Length];
		used[0] = true;

		var bestLength = double.MaxValue;

		bestLength = FindBestOrderRecursive(checkpoints, currentOrder, used, 1, 0.0, bestLength, bestOrder);

		return bestOrder;
	}

	private static double FindBestOrderRecursive(
		Point[] checkpoints,
		int[] currentOrder,
		bool[] used,
		int position,
		double currentLength,
		double bestLength,
		int[] bestOrder)
	{
		if (position == checkpoints.Length)
		{
			return UpdateBestOrderIfBetter(currentOrder, currentLength, bestLength, bestOrder);
		}
		for (var i = 0; i < checkpoints.Length; i++)
		{
			if (used[i]) continue;
			var prevIndex = currentOrder[position - 1];
			var newLength = currentLength + checkpoints[prevIndex].DistanceTo(checkpoints[i]);
			if (newLength >= bestLength)
				continue;
			currentOrder[position] = i;
			used[i] = true;
			bestLength = FindBestOrderRecursive(checkpoints, currentOrder, used, position + 1, newLength, bestLength, bestOrder);
			used[i] = false;
		}
		return bestLength;
	}

	private static double UpdateBestOrderIfBetter(
		int[] currentOrder,
		double currentLength,
		double bestLength,
		int[] bestOrder)
	{
		if (currentLength < bestLength)
		{
			bestLength = currentLength;
			Array.Copy(currentOrder, bestOrder, currentOrder.Length);
		}
		return bestLength;
	}
}
