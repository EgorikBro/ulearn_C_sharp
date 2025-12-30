using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGoogle;

public class Indexer : IIndexer
{
    private static readonly char[] Delimiters = { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };
    private readonly Dictionary<string, Dictionary<int, List<int>>> index = new();
    private readonly Dictionary<int, HashSet<string>> documentWords = new();

    public void Add(int id, string documentText)
    {
        if (documentWords.ContainsKey(id))
            Remove(id);

        var words = new HashSet<string>();
        documentWords[id] = words;

        IndexWords(id, documentText, words);
    }

    private void IndexWords(int id, string documentText, HashSet<string> words)
    {
        var start = -1;
        for (var i = 0; i < documentText.Length; i++)
        {
            if (Delimiters.Contains(documentText[i]))
            {
                if (start != -1)
                {
                    AddFoundWord(id, documentText, start, i, words);
                    start = -1;
                }
            }
            else
            {
                if (start == -1)
                    start = i;
            }
        }

        if (start != -1)
        {
            AddFoundWord(id, documentText, start, documentText.Length, words);
        }
    }

    private void AddFoundWord(int id, string documentText, int start, int end, HashSet<string> words)
    {
        var word = documentText.Substring(start, end - start);
        AddWord(id, word, start);
        words.Add(word);
    }

    private void AddWord(int id, string word, int position)
    {
        if (!index.TryGetValue(word, out var docMap))
        {
            docMap = new Dictionary<int, List<int>>();
            index[word] = docMap;
        }

        if (!docMap.TryGetValue(id, out var positions))
        {
            positions = new List<int>();
            docMap[id] = positions;
        }

        positions.Add(position);
    }

    public List<int> GetIds(string word)
    {
        if (index.TryGetValue(word, out var docMap))
        {
            return new List<int>(docMap.Keys);
        }
        return new List<int>();
    }

    public List<int> GetPositions(int id, string word)
    {
        if (index.TryGetValue(word, out var docMap))
        {
            if (docMap.TryGetValue(id, out var positions))
            {
                return new List<int>(positions);
            }
        }
        return new List<int>();
    }

    public void Remove(int id)
    {
        if (documentWords.TryGetValue(id, out var words))
        {
            foreach (var word in words)
            {
                if (index.TryGetValue(word, out var docMap))
                {
                    docMap.Remove(id);
                }
            }
            documentWords.Remove(id);
        }
    }
}
