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
        // Тело метода изменять не нужно
        Test(input, expectedOutput);
      }
