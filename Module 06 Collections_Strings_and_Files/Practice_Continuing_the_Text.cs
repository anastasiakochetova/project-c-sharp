using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    public static class TextGeneratorTask
    {
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            var resultWords = new List<string>(
                phraseBeginning.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            for (int step = 0; step < wordsCount; step++)
            {
                var next = FindNextWord(resultWords, nextWords);
                if (next == null)
                    break;
                resultWords.Add(next);
            }

            return string.Join(" ", resultWords);
        }

        private static string FindNextWord(
            List<string> currentPhrase,
            Dictionary<string, string> nextWords)
        {
            if (currentPhrase.Count >= 2)
            {
                var pairKey = ComposeBigramKey(currentPhrase);
                if (nextWords.TryGetValue(pairKey, out var continuation))
                    return continuation;
            }
            var singleKey = currentPhrase.Last();
            if (nextWords.TryGetValue(singleKey, out var next))
                return next;
            return null;
        }

        private static string ComposeBigramKey(List<string> words)
        {
            return words[words.Count - 2] + " " + words[words.Count - 1];
        }
    }
}
