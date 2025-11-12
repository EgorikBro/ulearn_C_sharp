using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            var words = phraseBeginning.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            for (var i = 0; i < wordsCount; i++)
            {
                var nextWord = GetNextWord(nextWords, words);
                if (nextWord == null)
                    break;

                words.Add(nextWord);
            }

            return string.Join(" ", words);
        }

        private static string? GetNextWord(Dictionary<string, string> nextWords, List<string> words)
        {
            if (words.Count >= 2)
            {
                var lastTwo = GetLastWords(words, 2);
                if (nextWords.ContainsKey(lastTwo))
                    return nextWords[lastTwo];
            }

            var lastOne = GetLastWords(words, 1);
            if (nextWords.ContainsKey(lastOne))
                return nextWords[lastOne];

            return null;
        }

        private static string GetLastWords(List<string> words, int count)
		{
			var startIndex = words.Count - count;
			return string.Join(" ", words[startIndex..]);
		}	
    }
}
