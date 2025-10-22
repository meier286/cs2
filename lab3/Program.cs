class Text
{
    List<string> text = new List<string>();

    public void writeToText(string inputtext)
    {
        text.Add(inputtext);
    }
}

class Sentence
{
    List<string> sentences = new List<string>();

    public void writeToSentence(string sentence)
    {
        sentences.Add(sentence);
    }
}

class Word
{
    List<string> words = new List<string>();

    public void writeToWord(string word)
    {
        words.Add(word);
    }
}

class Punctuation
{
    List<char> punctuations = new List<char>();

    private static readonly HashSet<char> PunctuationChars = new HashSet<char>
    {
        '.', ',', '!', '?', ';', ':', '(', ')', '[', ']', '{', '}',
        '"', '…', '«', '»', '—'
    };
    public static bool isPunctuation(char c)
    {
        return PunctuationChars.Contains(c) || char.IsPunctuation(c);
    }

    public void writeToPunctuation(string punctuation)
    {
        for (int i = 0; i < punctuation.Length; i++)
        {
            if (isPunctuation(punctuation[i]))
            {
                punctuations.Add(punctuation[i]);
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
        text.writeToText(ReadAllText("text.txt"));
    }
}