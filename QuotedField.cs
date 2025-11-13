using NUnit.Framework;
using System.Text;

namespace TableParser;

[TestFixture]
public class QuotedFieldTaskTests
{
    [TestCase("''", 0, "", 2)]
    [TestCase("'a'", 0, "a", 3)]
    [TestCase("\"abc\"", 0, "abc", 5)]
    [TestCase("\"a 'b' 'c' d\"", 0, "a 'b' 'c' d", 13)]
    [TestCase("'a \"b\" c'", 0, "a \"b\" c", 9)]
    [TestCase("\"a \\\"b\\\" c\"", 0, "a \"b\" c", 11)]
    [TestCase("\"a \\\\ b\"", 0, "a \\ b", 8)]
    [TestCase("\"abc", 0, "abc", 4)]
    [TestCase("a\"b c d e\"f", 1, "b c d e", 9)]
    public void Test(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
        NUnit.Framework.Legacy.ClassicAssert.AreEqual(
            new Token(expectedValue, startIndex, expectedLength),
            actualToken);
    }
}


class QuotedFieldTask
{
    public static Token ReadQuotedField(string line, int startIndex)
    {
        var quote = line[startIndex];
        var (value, i) = ReadQuotedFieldValue(line, startIndex + 1, quote);
        var length = i - startIndex;
        return new Token(value, startIndex, length);
    }

    private static (string, int) ReadQuotedFieldValue(string line, int i, char quote)
    {
        var value = new StringBuilder();

        while (i < line.Length)
        {
            if (line[i] == '\\' && i + 1 < line.Length)
            {
                value.Append(line[i + 1]);
                i += 2;
            }
            else if (line[i] == quote)
            {
                i++;
                break;
            }
            else
            {
                value.Append(line[i]);
                i++;
            }
        }

        return (value.ToString(), i);
    }
}
