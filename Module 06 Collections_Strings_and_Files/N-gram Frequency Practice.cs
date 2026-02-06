using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    public static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> textData)
        {
            var frequencyMap = new Dictionary<string, Dictionary<string, int>>();
            CollectStatistics(textData, frequencyMap);
            return BuildModel(frequencyMap);
		}

        private static void CollectStatistics(
            List<List<string>> textData,
            Dictionary<string, Dictionary<string, int>> frequencyMap)
        {
            foreach (var phrase in textData)
                ProcessPhrase(phrase, frequencyMap);
        }

        private static void ProcessPhrase(
            List<string> phrase,
            Dictionary<string, Dictionary<string, int>> frequencyMap)
        {
            for (int index = 0; index < phrase.Count; index++)
            {
                AddBigram(phrase, index, frequencyMap);
                AddTrigram(phrase, index, frequencyMap);
            }
        }

        private static void AddBigram(
            List<string> phrase,
            int index,
            Dictionary<string, Dictionary<string, int>> frequencyMap)
        {
            if (index + 1 < phrase.Count)
                Register(frequencyMap, phrase[index], phrase[index + 1]);
        }

        private static void AddTrigram(
            List<string> phrase,
            int index,
            Dictionary<string, Dictionary<string, int>> frequencyMap)
        {
            if (index + 2 < phrase.Count)
            {
                var prefix = phrase[index] + " " + phrase[index + 1];
                Register(frequencyMap, prefix, phrase[index + 2]);
            }
        }

        private static Dictionary<string, string> BuildModel(
            Dictionary<string, Dictionary<string, int>> frequencyMap)
        {
            var model = new Dictionary<string, string>();

            foreach (var entry in frequencyMap)
                model[entry.Key] = ChooseBestContinuation(entry.Value);

            return model;
        }

        private static string ChooseBestContinuation(
            Dictionary<string, int> variants)
        {
            return variants
                .OrderByDescending(x => x.Value)
                .ThenBy(x => x.Key, StringComparer.Ordinal)
                .First()
                .Key;
        }

        private static void Register(
            Dictionary<string, Dictionary<string, int>> storage,
            string prefix,
            string continuation)
        {
            if (!storage.TryGetValue(prefix, out var options))
            {
                options = new Dictionary<string, int>();
                storage[prefix] = options;
            }

            if (!options.ContainsKey(continuation))
                options[continuation] = 0;

            options[continuation]++;
        }
    }
}
