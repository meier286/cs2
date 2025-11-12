using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("text")] 
public class TextXml
{
    [XmlElement("sentence")]
    public List<SentenceXml> Sentences { get; set; } = new List<SentenceXml>();
}

[Serializable]
public class SentenceXml
{
    [XmlElement("word")]
    public List<string> Words { get; set; } = new List<string>();
}

class Text
{
    public List<string> text = new List<string>();

    public void WriteToText(string inputtext)
    {
        text.Add(inputtext);
    }

    public string getFirstValue(List<string> text) 
    {
        return text[0];
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

    public void RemoveWordsStartingWithConsonant(Sentence sentenceProcessor, int wordLength)
    {
        Console.WriteLine($"Удаление слов длины {wordLength}, начинающихся с согласной буквы...");
        
        var consonants = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxz";
        int removedCount = 0;
        
        for (int i = 0; i < sentenceProcessor.sentences.Count; i++)
        {
            var sentence = sentenceProcessor.sentences[i];
            sentence = RemoveNumberedListsFromSentence(sentence);
            var words = ExtractWords(sentence);
            var wordsToKeep = new List<string>();
            
            foreach (var word in words)
            {
                if (word.Length == wordLength && !string.IsNullOrEmpty(word) && 
                    consonants.Contains(char.ToLower(word[0])))
                {
                    removedCount++;
                }
                else
                {
                    wordsToKeep.Add(word);
                }
            }
            
            sentenceProcessor.sentences[i] = string.Join(" ", wordsToKeep);
        }
        
        Console.WriteLine($"Удалено слов: {removedCount}");
    }

    public void ReplaceWordsInSentence(Sentence sentenceProcessor, int sentenceIndex, int wordLength, string replacement)
    {
        if (sentenceIndex < 0 || sentenceIndex >= sentenceProcessor.sentences.Count)
        {
            Console.WriteLine("Ошибка: неверный индекс предложения");
            return;
        }

        Console.WriteLine($"Замена слов длины {wordLength} в предложении {sentenceIndex + 1} на '{replacement}'");
        
        var sentence = sentenceProcessor.sentences[sentenceIndex];
        sentence = RemoveNumberedListsFromSentence(sentence);
        var words = ExtractWords(sentence);
        var modifiedWords = new List<string>();
        
        foreach (var word in words)
        {
            if (word.Length == wordLength)
            {
                modifiedWords.Add(replacement);
            }
            else
            {
                modifiedWords.Add(word);
            }
        }
        
        sentenceProcessor.sentences[sentenceIndex] = string.Join(" ", modifiedWords);
        Console.WriteLine($"Результат: {sentenceProcessor.sentences[sentenceIndex]}");
    }

    public void RemoveStopWords(Sentence sentenceProcessor, HashSet<string> stopWords)
    {
        Console.WriteLine("Удаление стоп-слов...");
        int remCount = 0;
        
        for (int i = 0; i < sentenceProcessor.sentences.Count; i++)
        {
            var sentence = sentenceProcessor.sentences[i];
            sentence = RemoveNumberedListsFromSentence(sentence);
            var words = ExtractWords(sentence);
            var wordsToKeep = new List<string>();
            
            foreach (var word in words)
            {
                if (stopWords.Contains(word.ToLower()))
                {
                    remCount++;
                }
                else
                {
                    wordsToKeep.Add(word);
                }
            }
            
            sentenceProcessor.sentences[i] = string.Join(" ", wordsToKeep);
        }
        
        Console.WriteLine($"Удалено стоп-слов: {remCount}");
    }

    public void ExportToXml(Sentence sentenceProcessor, string filePath)
    {
        try
        {
            var textXml = new TextXml();
            
            foreach (var sentence in sentenceProcessor.sentences)
            {
                var sentenceXml = new SentenceXml();
                var cleanedSentence = RemoveNumberedListsFromSentence(sentence);
                var words = ExtractWords(cleanedSentence);
                
                foreach (var word in words)
                {
                    sentenceXml.Words.Add(word);
                }
                
                textXml.Sentences.Add(sentenceXml);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(TextXml));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, textXml);
            }
            Console.WriteLine($"Текст успешно экспортирован в {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка экспорта в XML: {ex.Message}");
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

    private string RemoveNumberedListsFromSentence(string sentence)
    {
        if (string.IsNullOrEmpty(sentence))
            return sentence;

        string numberedListPattern = @"\s+(?:\d+[\.\)]|[IVXLCDM]+\.)\s+";
        
        string result = Regex.Replace(sentence, numberedListPattern, " ");
        
        string startPattern = @"^(?:\d+[\.\)]|[IVXLCDM]+\.)\s+";
        result = Regex.Replace(result, startPattern, "");
        
        result = Regex.Replace(result, @"\s+", " ");
        
        return result.Trim();
    }
}

class Sentence
{
    public List<string> sentences = new List<string>();
    
    public void WriteToSentence(string text)
    {
        string cleanedText = RemoveNumberedLists(text);
        
        string pattern = @"(?<=[.!?])\s+(?=[А-ЯA-Z])";
        string[] temp = Regex.Split(cleanedText, pattern);
        foreach (string sent in temp)
        {   
            if (!string.IsNullOrWhiteSpace(sent))
            {
                sentences.Add(sent.Trim());
            }
        }
    }
    
    private string RemoveNumberedLists(string text)
    {
        string numberedListPattern = @"(?:^|\s)(?:\d+[\.\)]|[IVXLCDM]+\.[\s]*)(?=\s|$)";
        
        string result = Regex.Replace(text, numberedListPattern, " ");
        
        result = Regex.Replace(result, @"\s+", " ");
        
        return result.Trim();
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

    public static HashSet<string> LoadStopWords(string filePath)
    {
        var stopWords = new HashSet<string>();
        try
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        stopWords.Add(line.Trim().ToLower());
                    }
                }
                Console.WriteLine($"Загружено {stopWords.Count} стоп-слов из {filePath}");
            }
            else
            {
                Console.WriteLine($"Файл {filePath} не найден");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки стоп-слов: {ex.Message}");
        }
        return stopWords;
    }
    
    static void Main()
    {
        Text text = new Text();
        Sentence sentence = new Sentence();
        Word word = new Word();
        Punctuation punctuation = new Punctuation();
        
        string fileContent = ReadAllText("text.txt");
        if (string.IsNullOrEmpty(fileContent))
        {
            Console.WriteLine("Файл text.txt не найден или пуст");
            return;
        }
        
        text.WriteToText(fileContent);
        punctuation.WriteToPunctuation(fileContent);
        sentence.WriteToSentence(fileContent);
        word.WriteToWord(fileContent);
        
        Console.WriteLine("=== ИСХОДНЫЕ ДАННЫЕ ===");
        Console.WriteLine("Предложения:");
        for (int i = 0; i < sentence.sentences.Count; i++)
        {
            Console.WriteLine($" {i + 1}. {sentence.sentences[i]}");
        }
        
        Console.WriteLine("\n=== ВЫПОЛНЕНИЕ ЗАДАЧ ===");
        
        text.PrintSentencesByWordCount(sentence);
        Console.WriteLine();
        
        text.PrintSentencesByLength(sentence);
        Console.WriteLine();
        
        text.FindWordsInQuestions(sentence, 3);
        Console.WriteLine();
        
        text.RemoveWordsStartingWithConsonant(sentence, 3);
        Console.WriteLine();
        
        if (sentence.sentences.Count > 0)
        {
            text.ReplaceWordsInSentence(sentence, 0, 2, "[ЗАМЕНА]");
        }
        Console.WriteLine();
        
        var russianStopWords = LoadStopWords("russian_stopwords.txt");
        var englishStopWords = LoadStopWords("english_stopwords.txt");
        var allStopWords = new HashSet<string>(russianStopWords.Concat(englishStopWords));
        
        if (allStopWords.Count > 0)
        {
            text.RemoveStopWords(sentence, allStopWords);
        }
        else
        {
            Console.WriteLine("Стоп-слова не загружены, пропускаем удаление");
        }
        Console.WriteLine();
        
        text.ExportToXml(sentence, "text_export.xml");
        
        Console.WriteLine("\n=== ФИНАЛЬНЫЙ РЕЗУЛЬТАТ ===");
        for (int i = 0; i < sentence.sentences.Count; i++)
        {
            Console.WriteLine($" {i + 1}. {sentence.sentences[i]}");
        }
    }
}