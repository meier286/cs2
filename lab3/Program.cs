using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Text
{
    public List<string> text = new List<string>();

    public void WriteToText(string inputtext)
    {
        text.Add(inputtext);
    }

    public void PrintSentencesByWordCount(Sentence sentenceProcessor)
    {
        Console.WriteLine("Предложения по возрастанию количества слов: ");
        
        var sortedSentences = sentenceProcessor.sentences.OrderBy(s => CountWords(s));
        
        foreach (var sentence in sortedSentences)
        {
            Console.WriteLine($"[{CountWords(sentence)} слов] {sentence}");
        }
    }

    public void PrintSentencesByLength(Sentence sentenceProcessor)
    {
        Console.WriteLine("Предложения по возрастанию длины: ");
        
        var sortedSentences = sentenceProcessor.sentences.OrderBy(s => s.Length);
        
        foreach (var sentence in sortedSentences)
        {
            Console.WriteLine($"[{sentence.Length} символов] {sentence}");
        }
    }

    public void FindWordsInQuestions(Sentence sentenceProcessor, int wordLength)
    {
        Console.WriteLine($"Слова длины {wordLength} в вопросительных предложениях: ");
        
        var uniqueWords = new HashSet<string>();
        
        foreach (var sentence in sentenceProcessor.sentences)
        {
            if (sentence.Trim().EndsWith("?"))
            {
                var words = ExtractWords(sentence);
                foreach (var word in words)
                {
                    if (word.Length == wordLength)
                    {
                        uniqueWords.Add(word.ToLower());
                    }
                }
            }
        }

        if (uniqueWords.Count == 0)
        {
            Console.WriteLine("Слова заданной длины не найдены в вопросительных предложениях.");
        }
        else
        {
            foreach (var word in uniqueWords.OrderBy(w => w))
            {
                Console.WriteLine(word);
            }
            Console.WriteLine($"Всего найдено уникальных слов: {uniqueWords.Count}");
        }
    }

    private int CountWords(string text)
    {
        string pattern = @"\b\w+\b";
        return Regex.Matches(text, pattern).Count;
    }

    private string[] ExtractWords(string text)
    {
        string pattern = @"\b\w+\b";
        MatchCollection matches = Regex.Matches(text, pattern);
        return matches.Cast<Match>().Select(m => m.Value).ToArray();
    }
}

class Sentence
{
    public List<string> sentences = new List<string>();
    
    public void WriteToSentence(string text)
    {
        string pattern = @"(?<=[.!?])\s+(?=[А-ЯA-Z])";
        string[] temp = Regex.Split(text, pattern);
        foreach (string sent in temp)
        {   
            if (!string.IsNullOrWhiteSpace(sent))
            {
                sentences.Add(sent.Trim());
            }
        }
    }
}

class Word
{
    public List<string> words = new List<string>();

    public void WriteToWord(string text)
    {
        string pattern = @"\b\w+\b";
        MatchCollection temp = Regex.Matches(text, pattern);

        foreach (Match match in temp)
        {
            words.Add(match.Value);
        }
    }
}

class Punctuation
{
    public List<char> punctuations = new List<char>();

    private static readonly HashSet<char> PunctuationChars = new HashSet<char>
    {
        '.', ',', '!', '?', ';', ':', '(', ')', '[', ']', '{', '}',
        '"', '…', '«', '»', '—'
    };
    
    public static bool IsPunctuation(char c)
    {
        return PunctuationChars.Contains(c) || char.IsPunctuation(c);
    }

    public void WriteToPunctuation(string text)
    {
        foreach (char c in text)
        {
            if (IsPunctuation(c))
            {
                punctuations.Add(c);
            }
        }
    }
}

class Program
{
    public static string ReadAllText(string filePath)
    {
        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
            return "";
        }
    }
    
    static void Main()
    {
        Text text = new Text();
        Sentence sentence = new Sentence();
        Word word = new Word();
        Punctuation punctuation = new Punctuation();
        
        string fileContent = ReadAllText("text.txt");
        text.WriteToText(fileContent);
        
        punctuation.WriteToPunctuation(fileContent);
        sentence.WriteToSentence(fileContent);
        word.WriteToWord(fileContent);
        
        foreach (var sent in sentence.sentences)
        {
            Console.WriteLine(sent);
        }
        
        foreach (var w in word.words)
        {
            Console.WriteLine(w);
        }
        
        foreach (var p in punctuation.punctuations)
        {
            Console.WriteLine(p);
        }

        text.PrintSentencesByWordCount(sentence);
        Console.WriteLine();
        text.PrintSentencesByLength(sentence);
        Console.WriteLine();
        text.FindWordsInQuestions(sentence, 3); 
    }
}