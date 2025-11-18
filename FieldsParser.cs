using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expected)
        {
            var result = FieldsParserTask.ParseLine(input);
            ClassicAssert.AreEqual(expected.Length, result.Count);

            for (var i = 0; i < expected.Length; i++)
                ClassicAssert.AreEqual(expected[i], result[i].Value);
        }

        [TestCase("text", new[] { "text" })]
		[TestCase("hello world", new[] { "hello", "world" })]
		[TestCase("a", new[] { "a" })]
		[TestCase("\"a\"b", new[] { "a", "b" })]
		[TestCase(@"""\\""", new[] { "\\" })]
		[TestCase("", new string[0])]
		[TestCase("a\"b\"", new[] { "a", "b" })]
		[TestCase("\"\\\"a\\\"\"", new[] { "\"a\"" })]
		[TestCase("'\\\'a\\\''", new[] { "'a'" })]
		[TestCase("'\"a\"", new[] { "\"a\"" })]
		[TestCase("a  b ", new[] { "a", "b" })]
		[TestCase("\'\'", new[] { "" })]
		[TestCase("\"'a' b\"", new[] { "'a' b" })]
		[TestCase("' ", new[] { " " })]
        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var tokens = new List<Token>();
            var position = 0;

            while (position < line.Length)
            {
                if (IsSpace(line[position]))
                {
                    position++;
                    continue;
                }

                var item = CaptureToken(line, position);
                tokens.Add(item);

                position = item.GetIndexNextToToken();
            }

            return tokens;
        }

        private static bool IsSpace(char c) => c == ' ';

        private static Token CaptureToken(string line, int index)
        {
            var c = line[index];

            return IsQuote(c)
                ? ReadQuotedField(line, index)
                : ReadUnquotedField(line, index);
        }

        private static bool IsQuote(char c) => c == '"' || c == '\'';

        private static Token ReadUnquotedField(string line, int start)
        {
            var sb = new StringBuilder();
            var p = start;

            while (p < line.Length)
            {
                var ch = line[p];
                if (IsSpace(ch) || IsQuote(ch))
                    break;

                sb.Append(ch);
                p++;
            }

            return new Token(sb.ToString(), start, sb.Length);
        }

        public static Token ReadQuotedField(string line, int start)
        {
            return QuotedFieldTask.ReadQuotedField(line, start);
        }
    }
}
