using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.That(actualResult.Count, Is.EqualTo(expectedResult.Length));

            for (int i = 0; i < expectedResult.Length; i++)
            {
                Assert.That(actualResult[i].Value, Is.EqualTo(expectedResult[i]));
            }
        }

        [TestCase("text", new[] { "text" })]
        [TestCase("", new string[0])]
        [TestCase("\"\\\"text\\\"\"", new[] { "\"text\"" })]
        [TestCase("'\\\'text\\\''", new[] { "'text'" })]
        [TestCase("'\"text\"", new[] { "\"text\"" })]
        [TestCase("hello  world ", new[] { "hello", "world" })]
        [TestCase("\"'hello' world\"", new[] { "'hello' world" })]
        [TestCase("\"hello\"world", new[] { "hello", "world" })]
        [TestCase(@"""\\""", new[] { "\\" })]
        [TestCase("hello\"world\"", new[] { "hello", "world" })]
        [TestCase("' ", new[] { " " })]
        [TestCase("\'\'", new[] { "" })]
        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    //Хранилище пробелов
    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var tokens = new List<Token>();

            int index = 0;

            while (index < line.Length)
            {
                if (line[index] == ' ')
                {
                    index++;
                    continue;
                }

                Token token = TakeToken(line, index);
                tokens.Add(token);

                index = token.GetIndexNextToToken();
            }

            return tokens;
        }

        //Проверка токена на "особенность"
        public static Token TakeToken(string line, int index)
        {
            if (index < line.Length && (line[index] == '"' || line[index] == '\''))
            {
                return ReadQuotedField(line, index);
            }
            else
            {
                return ReadField(line, index);
            }
        }

        // Сюда будем складывать все найденные символы
        private static Token ReadField(string line, int startIndex)
        {
            var sb = new StringBuilder();
            int i = startIndex;

            while (i < line.Length)
            {
                char symbol = line[i];

                if (symbol == ' ' || symbol == '"' || symbol == '\'')
                {
                    break;
                }

                sb.Append(symbol);
                i++;
            }

            return new Token(sb.ToString(), startIndex, sb.Length);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}
