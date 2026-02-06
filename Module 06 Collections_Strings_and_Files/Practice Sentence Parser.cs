// Вставьте сюда финальное содержимое файла SentencesParserTask.cs
namespace TextAnalysis;

static class SentencesParserTask
{
    private static readonly HashSet<char> SentenceDelimiters =
        new() { '.', '!', '?', ';', ':', '(', ')' };

    public static List<List<string>> ParseSentences(string text)
    {
        var sentences = new List<List<string>>();
        var currentSentence = new List<string>();
        var wordBuilder = new System.Text.StringBuilder();

        foreach (var c in text)
            ProcessChar(c, wordBuilder, currentSentence, sentences);

        FinalizeParsing(wordBuilder, currentSentence, sentences);

        return sentences;
    }

    private static void ProcessChar(
        char c,
        System.Text.StringBuilder wordBuilder,
        List<string> currentSentence,
        List<List<string>> sentences)
    {
        if (IsWordChar(c))
        {
            AddCharToWord(c, wordBuilder);
        }
        else if (IsSentenceDelimiter(c))
        {
            FinishWord(wordBuilder, currentSentence);
            FinishSentence(sentences, currentSentence);
        }
        else
        {
            FinishWord(wordBuilder, currentSentence);
        }
    }

    private static void FinalizeParsing(
        System.Text.StringBuilder wordBuilder,
        List<string> currentSentence,
        List<List<string>> sentences)
    {
        FinishWord(wordBuilder, currentSentence);
        FinishSentence(sentences, currentSentence);
    }

    private static bool IsWordChar(char c) =>
        char.IsLetter(c) || c == '\'';

    private static bool IsSentenceDelimiter(char c) =>
        SentenceDelimiters.Contains(c);

    private static void AddCharToWord(char c, System.Text.StringBuilder builder) =>
		builder.Append(c);

    private static void FinishWord(System.Text.StringBuilder builder, List<string> sentence)
    {
        if (builder.Length == 0)
            return;

        sentence.Add(builder.ToString().ToLower());
        builder.Clear();
    }

    private static void FinishSentence(List<List<string>> sentences, List<string> sentence)
    {
        if (sentence.Count == 0)
            return;

        sentences.Add(new List<string>(sentence));
        sentence.Clear();
    }
}
