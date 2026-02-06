// Вставьте сюда финальное содержимое файла QuotedFieldTask.cs
using NUnit.Framework;

namespace TableParser;

[TestFixture]
public class QuotedFieldTaskTests
{
    [TestCase("''", 0, "", 2)]
    [TestCase("'a'", 0, "a", 3)]
    [TestCase("\"a\"", 0, "a", 3)]
    [TestCase("\"a b c\"", 0, "a b c", 7)]
    [TestCase("\"a 'b' c\"", 0, "a 'b' c", 9)]
    [TestCase("'a \"b\" c'", 0, "a \"b\" c", 9)]
    [TestCase("\"a \\\"b\\\" c\"", 0, "a \"b\" c", 11)]
    [TestCase("'a \\'b\\' c'", 0, "a 'b' c", 11)]
    [TestCase("\"\\\\\"", 0, "\\", 4)]
    [TestCase("\"abc", 0, "abc", 4)]
    [TestCase("'a b'c d", 0, "a b", 5)]
    public void Test(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
        Assert.That(actualToken, Is.EqualTo(new Token(expectedValue, startIndex, expectedLength)));
    }
}

class QuotedFieldTask
{
    public static Token ReadQuotedField(string line, int startIndex)
    {
        char quote = line[startIndex];
        int index = startIndex + 1;
        var value = new System.Text.StringBuilder();

        while (index < line.Length)
        {
            if (IsEscape(line, index))
            {
                AppendEscaped(value, line, ref index);
                continue;
            }

            if (IsClosingQuote(line, index, quote))
                return MakeToken(startIndex, index + 1, value);

            value.Append(line[index]);
            index++;
        }

        return MakeToken(startIndex, index, value);
    }

    static bool IsEscape(string s, int i) =>
        s[i] == '\\' && i + 1 < s.Length;

    static void AppendEscaped(System.Text.StringBuilder sb, string s, ref int i)
    {
        i++;
        sb.Append(s[i]);
        i++;
    }

    static bool IsClosingQuote(string s, int i, char quote) =>
        s[i] == quote;

    static Token MakeToken(int start, int end, System.Text.StringBuilder sb) =>
        new Token(sb.ToString(), start, end - start);
}
