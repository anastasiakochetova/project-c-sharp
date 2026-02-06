// Вставьте сюда финальное содержимое файла Indexer.cs

using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketGoogle;

public class Indexer : IIndexer
{
    private readonly Dictionary<string, Dictionary<int, List<int>>> wordIndex;
    private readonly Dictionary<int, HashSet<string>> documentWords;
    private readonly char[] separators = { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };

    public Indexer()
    {
        wordIndex = new Dictionary<string, Dictionary<int, List<int>>>();
        documentWords = new Dictionary<int, HashSet<string>>();
    }

    public void Add(int id, string documentText)
    {
        EnsureDocumentNotExists(id);
        
        var positionsByWord = ParseDocument(documentText);
        AddDocumentToIndex(id, positionsByWord);
        RegisterDocumentWords(id, positionsByWord.Keys);
    }

    public void Remove(int id)
    {
        if (!documentWords.TryGetValue(id, out var words))
            return;

        RemoveDocumentFromIndex(id, words);
        UnregisterDocument(id);
    }

    public List<int> GetIds(string word)
    {
        return TryGetWordDocuments(word, out var documents) 
            ? documents.Keys.ToList() 
            : new List<int>();
    }

    public List<int> GetPositions(int id, string word)
    {
        if (TryGetWordPositions(word, id, out var positions))
            return new List<int>(positions);
        
        return new List<int>();
    }

    private void EnsureDocumentNotExists(int id)
    {
        if (documentWords.ContainsKey(id))
            Remove(id);
    }

    private Dictionary<string, List<int>> ParseDocument(string documentText)
    {
        var result = new Dictionary<string, List<int>>();
        var wordStart = -1;

        for (int position = 0; position < documentText.Length; position++)
        {
            ProcessCharacter(documentText, position, ref wordStart, result);
        }

        ProcessRemainingWord(documentText, ref wordStart, result);
        return result;
    }

    private void ProcessCharacter(string text, int position, ref int wordStart, 
                                  Dictionary<string, List<int>> result)
    {
        if (IsSeparator(text[position]))
        {
            FinalizeWordIfStarted(text, position, ref wordStart, result);
        }
        else
        {
            StartWordIfNeeded(position, ref wordStart);
        }
    }

    private void FinalizeWordIfStarted(string text, int currentPosition, ref int wordStart, 
                                       Dictionary<string, List<int>> result)
    {
        if (wordStart == -1) return;
        
        ExtractAndStoreWord(text, wordStart, currentPosition, result);
        wordStart = -1;
    }

    private void StartWordIfNeeded(int position, ref int wordStart)
    {
        if (wordStart == -1)
            wordStart = position;
    }

    private void ProcessRemainingWord(string text, ref int wordStart, 
                                      Dictionary<string, List<int>> result)
    {
        if (wordStart == -1) return;
        
        ExtractAndStoreWord(text, wordStart, text.Length, result);
    }

    private void ExtractAndStoreWord(string text, int start, int end, 
                                     Dictionary<string, List<int>> result)
    {
        var word = text.Substring(start, end - start);
        AddWordPosition(word, start, result);
    }

    private void AddWordPosition(string word, int position, Dictionary<string, List<int>> dictionary)
    {
        if (!dictionary.ContainsKey(word))
            dictionary[word] = new List<int>();
        
        dictionary[word].Add(position);
    }

    private bool IsSeparator(char c)
    {
        return separators.Contains(c);
    }

    private void AddDocumentToIndex(int id, Dictionary<string, List<int>> positionsByWord)
    {
        foreach (var entry in positionsByWord)
        {
            AddWordToIndex(entry.Key, id, entry.Value);
        }
    }

    private void AddWordToIndex(string word, int id, List<int> positions)
    {
        EnsureWordInIndex(word);
        EnsureDocumentInWordIndex(word, id);
        
        wordIndex[word][id].AddRange(positions);
    }

    private void EnsureWordInIndex(string word)
    {
        if (!wordIndex.ContainsKey(word))
            wordIndex[word] = new Dictionary<int, List<int>>();
    }

    private void EnsureDocumentInWordIndex(string word, int id)
    {
        if (!wordIndex[word].ContainsKey(id))
            wordIndex[word][id] = new List<int>();
    }

    private void RegisterDocumentWords(int id, IEnumerable<string> words)
    {
        documentWords[id] = new HashSet<string>(words);
    }

    private bool TryGetWordDocuments(string word, out Dictionary<int, List<int>> documents)
    {
        return wordIndex.TryGetValue(word, out documents);
    }

    private bool TryGetWordPositions(string word, int id, out List<int> positions)
    {
        positions = null;
        return wordIndex.TryGetValue(word, out var documents) && 
               documents.TryGetValue(id, out positions);
    }

    private void RemoveDocumentFromIndex(int id, HashSet<string> words)
    {
        foreach (var word in words)
        {
            RemoveDocumentFromWordIndex(word, id);
        }
    }

    private void RemoveDocumentFromWordIndex(string word, int id)
    {
        if (!wordIndex.TryGetValue(word, out var documents)) 
            return;
        
        documents.Remove(id);
        
        if (documents.Count == 0)
            wordIndex.Remove(word);
    }

    private void UnregisterDocument(int id)
    {
        documentWords.Remove(id);
    }

    public int GetTotalWordCount()
    {
        return wordIndex.Values.Sum(docs => docs.Values.Sum(positions => positions.Count));
    }
}
