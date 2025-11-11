using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();
            var counts = new Dictionary<string, Dictionary<string, int>>();

            CollectNGrams(text, counts);
            SelectMostFrequentNextWords(counts, result);

            return result;
        }

        private static void CollectNGrams(List<List<string>> text, Dictionary<string, Dictionary<string, int>> counts)
        {
            foreach (var sentence in text)
            {
                if (sentence.Count < 2)
                    continue;

                for (int i = 0; i < sentence.Count - 1; i++)
                {
                    AddNGram(counts, sentence, i, 1); // биграмма

                    if (i < sentence.Count - 2)
                        AddNGram(counts, sentence, i, 2); // триграмма
                }
            }
        }

        private static void AddNGram(Dictionary<string, Dictionary<string, int>> counts, List<string> sentence, int index, int n)
        {
            var prefix = string.Join(" ", sentence.Skip(index).Take(n));
            var nextWord = sentence[index + n];

            if (!counts.ContainsKey(prefix))
                counts[prefix] = new Dictionary<string, int>();

            if (!counts[prefix].ContainsKey(nextWord))
                counts[prefix][nextWord] = 0;

            counts[prefix][nextWord]++;
        }

        private static void SelectMostFrequentNextWords(Dictionary<string, Dictionary<string, int>> counts, Dictionary<string, string> result)
        {
            foreach (var prefix in counts.Keys)
            {
                var nextWords = counts[prefix];
                var bestNext = nextWords
                    .OrderByDescending(p => p.Value)
                    .ThenBy(p => p.Key, StringComparer.Ordinal)
                    .First().Key;

                result[prefix] = bestNext;
            }
        }
    }
}
