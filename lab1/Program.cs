/*Для операции search:
• выводим название организма и название белка
• если совпадений не найдено, выводим «NOT FOUND»
Для операции diff:
• выводим количество аминокислот, которыми различаются белки
Для операции mode:
•выводим наиболее часто встречающуюся аминокислоту и количество раз, которое она встречается в организме.
*/

struct GeneticData
{
    public string protein;
    public string organism;
    public string amino_acids;

    public string GetProtein()
    {
        return protein;
    }

    public void SetProtein(string newProtein)
    {
        protein = newProtein;
    }

    public string GetOrganism()
    {
        return organism;
    }

    public void SetOrganism(string newOrganism)
    {
        organism = newOrganism;
    }

    public string GetAminoAcids()
    {
        return amino_acids;
    }

    public void SetAminoAcids(string newAminoAcids)
    {
        amino_acids = newAminoAcids;
    }
}

class Program
{
    static List<GeneticData> data = new List<GeneticData>();
    static StreamWriter[] outputWriters = new StreamWriter[3];
    static string RLEncoding(string amino_acids)
    {
        string encoded = string.Empty;
        int i = 0;

        while (i < amino_acids.Length)
        {
            char currentChar = amino_acids[i];
            int count = 1;

            while (i + count < amino_acids.Length && amino_acids[i + count] == currentChar)
            {
                count++;
            }

            if (count > 2)
            {
                encoded += count.ToString() + currentChar;
            }
            else
            {
                encoded += new string(currentChar, count);
            }

            i += count;
        }

        return encoded;
    }


    static string RLDecoding(string amino_acids)
    {
        string result = "";
        int i = 0;

        while (i < amino_acids.Length)
        {
            if (char.IsDigit(amino_acids[i]))
            {
                string numStr = "";
                while (i < amino_acids.Length && char.IsDigit(amino_acids[i]))
                {
                    numStr += amino_acids[i];
                    i++;
                }

                char symbol = amino_acids[i];
                if (char.IsLetter(symbol))
                {
                    int count = int.Parse(numStr);
                    result += new string(symbol, count);
                }
                i++;
            }
            else if (char.IsLetter(amino_acids[i]))
            {
                result += amino_acids[i];
                i++;
            }
            else
            {
                i++;
            }
        }

        return result;
    }




    static void ReadGeneticData(string filename)
    {
        using (StreamReader reader = new StreamReader(filename))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;

                string[] fragments = line.Split('\t');
                if (fragments.Length < 3) continue;

                GeneticData protein = new GeneticData();
                protein.SetProtein(fragments[0]);
                protein.SetOrganism(fragments[1]);
                protein.SetAminoAcids(fragments[2]);
                data.Add(protein);
            }
        }
    }

    static void ReadHandleCommands(string filename, int fileIndex)
    {
        using (StreamReader reader = new StreamReader(filename))
        {
            int counter = 0;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;

                counter++;
                string[] command = line.Split('\t');

                switch (command[0])
                {
                    case "search":
                        HandleSearchCommand(command, counter, fileIndex);
                        break;
                    case "diff":
                        HandleDiffCommand(command, counter, fileIndex);
                        break;
                    case "mode":
                        HandleModeCommand(command, counter, fileIndex);
                        break;
                }
            }
        }
    }

    static void HandleSearchCommand(string[] command, int counter, int fileIndex)
    {
        outputWriters[fileIndex].WriteLine($"{counter.ToString("D3")}   {"search"}   {command[1]}");
        outputWriters[fileIndex].WriteLine("organism\tprotein");

        int index = Search(command[1]);
        if (index != -1)
            outputWriters[fileIndex].WriteLine($"{data[index].GetOrganism()}\t{data[index].GetProtein()}");
        else
            outputWriters[fileIndex].WriteLine("NOT FOUND");

        outputWriters[fileIndex].WriteLine("================================================");
    }

    static void HandleDiffCommand(string[] command, int counter, int fileIndex)
    {
        if (command.Length < 3) return;

        string protein1Name = command[1];
        string protein2Name = command[2];

        string aminoAcids1 = GetAminoAcids(protein1Name);
        string aminoAcids2 = GetAminoAcids(protein2Name);

        if (aminoAcids1 == null || aminoAcids2 == null)
        {
            outputWriters[fileIndex].WriteLine($"{counter.ToString("D3")}   {"diff"}   {protein1Name}   {protein2Name}");
            if (aminoAcids1 == null) outputWriters[fileIndex].WriteLine($"MISSING: {protein1Name}");
            if (aminoAcids2 == null) outputWriters[fileIndex].WriteLine($"MISSING: {protein2Name}");
            outputWriters[fileIndex].WriteLine("================================================");
            return;
        }

        Diff(aminoAcids1, aminoAcids2, fileIndex, counter, protein1Name, protein2Name);
    }

    static void HandleModeCommand(string[] command, int counter, int fileIndex)
    {
        if (command.Length < 2) return;

        string proteinName = command[1];
        string aminoAcids = GetAminoAcids(proteinName);

        if (aminoAcids == null)
        {
            outputWriters[fileIndex].WriteLine($"{counter.ToString("D3")} {"mode"} {proteinName}");
            outputWriters[fileIndex].WriteLine($"MISSING: {proteinName}");
            outputWriters[fileIndex].WriteLine("============================");
            return;
        }
        Mode(aminoAcids, fileIndex, counter, proteinName);
    }

    static string GetAminoAcids(string proteinName)
    {
        foreach (GeneticData item in data)
        {
            if (item.GetProtein().Equals(proteinName))
                return item.GetAminoAcids();
        }
        return null;
    }
    static int Search(string amino_acid)
    {
        /*Console.WriteLine("Введите количество живых организмов: ");
        int count = Int32.Parse(Console.ReadLine());
        GeneticData[] mas = new GeneticData[count];
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine("Организм " + (i + 1));
            GeneticData gendata = new GeneticData();
            Console.Write("Введите название белка: ");
            gendata.SetProtein(Console.ReadLine());
            Console.Write("Введите название организма: ");
            gendata.SetOrganism(Console.ReadLine());
            Console.Write("Введите аминокислоту: ");
            gendata.SetAminoAcids(Console.ReadLine());
            mas[i] = gendata;
        }

        Console.WriteLine("Введите интересующую последовательность аминокислот:");
        string searchAcids = Console.ReadLine();

        for (int i = 0; i < count; i++)
        {
            if (mas[i].GetAminoAcids() == searchAcids)
            {
                return "Организм: " + mas[i].GetOrganism() + ". Аминокислота: " + mas[i].GetProtein();
            }
        }

        return "NOT FOUND";*/

        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].GetAminoAcids().Contains(amino_acid))
                return i;
        }
        return -1;
    }

    static void Diff(string amino_acids1, string amino_acids2, int fileIndex = -1, int counter = -1, string protein1Name = "", string protein2Name = "")
    {
        var count1 = new Dictionary<char, int>();
        var count2 = new Dictionary<char, int>();

        for (int i = 0; i < amino_acids1.Length; i++)
        {
            char c = amino_acids1[i];
            if (count1.ContainsKey(c))
                count1[c]++;
            else
                count1[c] = 1;
        }

        for (int i = 0; i < amino_acids2.Length; i++)
        {
            char c = amino_acids2[i];
            if (count2.ContainsKey(c))
                count2[c]++;
            else
                count2[c] = 1;
        }

        var allChars = new HashSet<char>();
        foreach (char c in count1.Keys) allChars.Add(c);
        foreach (char c in count2.Keys) allChars.Add(c);

        int totalDifference = 0;

        foreach (char aminoAcid in allChars)
        {
            int countInFirst = 0;
            if (count1.ContainsKey(aminoAcid))
            {
                countInFirst = count1[aminoAcid];
            }

            int countInSecond = 0;
            if (count2.ContainsKey(aminoAcid))
            {
                countInSecond = count2[aminoAcid];
            }

            int difference = Math.Abs(countInFirst - countInSecond);
            totalDifference += difference;
        }

        if (fileIndex != -1)
        {
            outputWriters[fileIndex].WriteLine($"{counter.ToString("D3")} {"diff"} {protein1Name} {protein2Name}");
            outputWriters[fileIndex].WriteLine($"aminoacids difference: {totalDifference}");
            outputWriters[fileIndex].WriteLine("=====================");
        }
        else
        {
            Console.WriteLine("Разница по аминокислотам: " + totalDifference);
        }
    }

    static void Mode(string amino_acids, int fileIndex = -1, int counter = -1, string proteinName = "")
    {
        int biggestCount = 0;
        char biggestChar = amino_acids[0];

        for (int i = 0; i < amino_acids.Length; i++)
        {
            char currentChar = amino_acids[i];
            int count = 0;

            for (int j = 0; j < amino_acids.Length; j++)
            {
                if (amino_acids[j] == currentChar)
                    count++;
            }

            if (count > biggestCount)
            {
                biggestCount = count;
                biggestChar = currentChar;
            }
        }
        if (fileIndex != -1)
        {
            outputWriters[fileIndex].WriteLine($"{counter.ToString("D3")}   {"mode"}   {proteinName}");
            outputWriters[fileIndex].WriteLine("amino-acid occurs:");
            outputWriters[fileIndex].WriteLine($"{biggestChar}\t{biggestCount}");
            outputWriters[fileIndex].WriteLine("================================================");
        }
        else
        {
            Console.WriteLine("Аминокислота " + biggestChar + " встречаается " + biggestCount + " раз.");
        }
    }




    static void Main(string[] args)
    {
        outputWriters[0] = new StreamWriter("output_0.txt");
        outputWriters[1] = new StreamWriter("output_1.txt");
        outputWriters[2] = new StreamWriter("output_2.txt");

        try
        {
            for (int i = 0; i < 3; i++)
            {
                data.Clear();

                string sequencesFile = $"sequences.{i}.txt";
                string commandsFile = $"commands.{i}.txt";

                if (File.Exists(sequencesFile) && File.Exists(commandsFile))
                {
                    ReadGeneticData(sequencesFile);
                    ReadHandleCommands(commandsFile, i);
                }
                else
                {
                    Console.WriteLine($"Файлы {sequencesFile} или {commandsFile} не найдены");
                }

            }
        }
        finally
        {
            foreach (var writer in outputWriters)
            {
                writer?.Close();
            }
        }
    }
}

/*gendata.SetProtein(Console.ReadLine());
Console.Write("Введите название организма: ");
gendata.SetOrganism(Console.ReadLine());
Console.Write("Введите аминокислоту: ");
gendata.SetAminoAcids(Console.ReadLine());

bool isMenu = true;
while (isMenu)
{
    Console.WriteLine("Выберите режим работы:");
    Console.WriteLine("1. Кодировать аминокислоту.");
    Console.WriteLine("2. Декодировать аминокислоту.");
    Console.WriteLine("3. Вывести наиболее чаасто встречающуюся аминокислоту.");
    Console.WriteLine("4. Проверить различия аминокислот в составе двух белков.");
    Console.WriteLine("5. Найти организм по аминокислоте.");
    Console.WriteLine("0. Выйти из программы.");
    Console.WriteLine("-------------------------------");
    string choice = Console.ReadLine();
    Console.WriteLine("-------------------------------");

    switch (choice)
    {
        case "1":
            {
                gendata.SetAminoAcids(RLEncoding(gendata.GetAminoAcids()));
                Console.WriteLine("Аминокислота закодирована в " + gendata.GetAminoAcids() + "\n");
                break;
            }

        case "2":
            {
                gendata.SetAminoAcids(RLDecoding(gendata.GetAminoAcids()).ToString());
                Console.WriteLine("Аминокислота декодирована в " + gendata.GetAminoAcids() + "\n");
                break;
            }

        case "3":
            {
                Mode(gendata.GetAminoAcids());
                break;
            }

        case "4":
            {
                GeneticData gendata2 = new GeneticData();
                Console.Write("Введите название белка: ");
                gendata2.SetProtein(Console.ReadLine());
                Console.Write("Введите название организма: ");
                gendata2.SetOrganism(Console.ReadLine());
                Console.Write("Введите аминокислоту: ");
                gendata2.SetAminoAcids(Console.ReadLine());
                Diff(gendata.amino_acids, gendata2.amino_acids);
                break;
            }

        case "5":
            {
                Console.WriteLine(Search());
                break;
            }

        case "0":
            {
                isMenu = false;
                break;
            }
        default:
            {
                Console.WriteLine("Проверьте и повторите ввод.");
                break;
            }
    }
}*/

