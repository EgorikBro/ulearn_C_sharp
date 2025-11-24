namespace Passwords;

public class CaseAlternatorTask
{
	public static List<string> AlternateCharCases(string lowercaseWord)
	{
		var result = new List<string>();
		AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
		return result;
	}

	static void AlternateCharCases(char[] word, int startIndex, List<string> result)
	{
		if (startIndex >= word.Length)
		{
			result.Add(new string(word));
			return;
		}

		if (!char.IsLetter(word[startIndex]))
		{
			AlternateCharCases(word, startIndex + 1, result);
			return;
		}

		ProcessLetterChar(word, startIndex, result);
	}

	static void ProcessLetterChar(char[] word, int startIndex, List<string> result)
	{
		var lower = char.ToLower(word[startIndex]);
		var upper = char.ToUpper(word[startIndex]);

		if (lower != upper)
		{
			word[startIndex] = lower;
			AlternateCharCases(word, startIndex + 1, result);

			word[startIndex] = upper;
			AlternateCharCases(word, startIndex + 1, result);

			word[startIndex] = lower;
		}
		
		else
		{
			AlternateCharCases(word, startIndex + 1, result);
		}
	}
}
