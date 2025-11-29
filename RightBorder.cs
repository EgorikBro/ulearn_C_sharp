using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete;

public class RightBorderTask
{
	public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
	{
		while (right - left > 1)
		{
			var middle = left + (right - left) / 2;

			var comparison = string.Compare(phrases[middle], prefix, StringComparison.InvariantCultureIgnoreCase);
			var startsWithPrefix = phrases[middle].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase);

			if (comparison <= 0 || startsWithPrefix)
				left = middle;
			else
				right = middle;
		}

		return right;
	}
}
