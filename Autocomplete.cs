using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete;

internal class AutocompleteTask
{
	public static string? FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
	{
		var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
		if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
			return phrases[index];
            
		return null;
	}

	public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
	{
		var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
		var startIndex = leftBorder + 1;
		
		if (startIndex >= phrases.Count)
			return Array.Empty<string>();
		
		var result = new List<string>();
		for (int i = startIndex; i < phrases.Count && result.Count < count; i++)
		{
			if (phrases[i].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
			{
				result.Add(phrases[i]);
			}
			else break;
		}
		
		return result.ToArray();
	}

	public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
	{
		var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
		var rightBorder = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);
		
		var count = rightBorder - leftBorder - 1;
		return count > 0 ? count : 0;
	}
}

[TestFixture]
public class AutocompleteTests
{
	[Test]
	public void TopByPrefix_IsEmpty_WhenNoPhrases()
	{
		var phrases = Array.Empty<string>();
		var actualTopWords = AutocompleteTask.GetTopByPrefix(phrases, "prefix", 10);
		Assert.That(actualTopWords, Is.Empty);
	}

	[Test]
	public void TopByPrefix_IsEmpty_WhenNoMatches()
	{
		var phrases = new[] { "apple", "banana", "cherry" };
		var actualTopWords = AutocompleteTask.GetTopByPrefix(phrases, "xyz", 10);
		Assert.That(actualTopWords, Is.Empty);
	}

	[Test]
	public void TopByPrefix_ReturnsCorrectCount()
	{
		var phrases = new[] { "apple", "application", "apply", "banana", "cherry" };
		var actualTopWords = AutocompleteTask.GetTopByPrefix(phrases, "app", 2);
		Assert.That(actualTopWords.Length, Is.EqualTo(2));
		Assert.That(actualTopWords[0], Is.EqualTo("apple"));
		Assert.That(actualTopWords[1], Is.EqualTo("application"));
	}

	[Test]
	public void TopByPrefix_ReturnsAllMatches_WhenLessThanCount()
	{
		var phrases = new[] { "apple", "application", "banana", "cherry" };
		var actualTopWords = AutocompleteTask.GetTopByPrefix(phrases, "app", 10);
		Assert.That(actualTopWords.Length, Is.EqualTo(2));
		Assert.That(actualTopWords[0], Is.EqualTo("apple"));
		Assert.That(actualTopWords[1], Is.EqualTo("application"));
	}

	[Test]
	public void TopByPrefix_IsCaseInsensitive()
	{
		var phrases = new[] { "Apple", "application", "BANANA", "cherry" };
		var actualTopWords = AutocompleteTask.GetTopByPrefix(phrases, "APP", 10);
		Assert.That(actualTopWords.Length, Is.EqualTo(2));
		Assert.That(actualTopWords[0], Is.EqualTo("Apple"));
		Assert.That(actualTopWords[1], Is.EqualTo("application"));
	}

	[Test]
	public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
	{
		var phrases = new[] { "apple", "banana", "cherry" };
		var expectedCount = phrases.Length;
		var actualCount = AutocompleteTask.GetCountByPrefix(phrases, "");
		Assert.That(actualCount, Is.EqualTo(expectedCount));
	}

	[Test]
	public void CountByPrefix_ReturnsZero_WhenNoMatches()
	{
		var phrases = new[] { "apple", "banana", "cherry" };
		var actualCount = AutocompleteTask.GetCountByPrefix(phrases, "xyz");
		Assert.That(actualCount, Is.EqualTo(0));
	}

	[Test]
	public void CountByPrefix_ReturnsCorrectCount()
	{
		var phrases = new[] { "apple", "application", "apply", "banana", "cherry" };
		var actualCount = AutocompleteTask.GetCountByPrefix(phrases, "app");
		Assert.That(actualCount, Is.EqualTo(3));
	}

	[Test]
	public void CountByPrefix_IsCaseInsensitive()
	{
		var phrases = new[] { "Apple", "application", "APPLY", "banana" };
		var actualCount = AutocompleteTask.GetCountByPrefix(phrases, "APP");
		Assert.That(actualCount, Is.EqualTo(3));
	}

	[Test]
	public void CountByPrefix_ReturnsZero_WhenEmptyPhrases()
	{
		var phrases = Array.Empty<string>();
		var actualCount = AutocompleteTask.GetCountByPrefix(phrases, "prefix");
		Assert.That(actualCount, Is.EqualTo(0));
	}
}
