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
		string decoded = string.Empty;
		int i = 0;

		while (i < amino_acids.Length)
		{
			if (char.IsDigit(amino_acids[i]))
			{
				string numStr = string.Empty;

				while (char.IsDigit(amino_acids[i]))
				{
					numStr += amino_acids[i];
					i++;
				}

				if (int.TryParse(numStr, out int count))
				{
					decoded += new string(amino_acids[i], count);
					i++;
				}
			}
			else
			{
				decoded += amino_acids[i];
				i++;
			}
		}
		return decoded;
	}


	static void Search()
	{

	}

	static void Diff()
	{

	}

	static void Mode(string amino_acids)
	{
		int biggestCount = 1;
		char biggestChar = amino_acids[0];
		amino_acids = RLEncoding(amino_acids);


		Console.WriteLine("Аминокислота" + biggestChar + " встречаается " + biggestCount + " раз.");
	}

	static void Main(string[] args)
	{
		GeneticData gendata = new GeneticData();
		Console.Write("Введите название белка: ");
		gendata.SetProtein(Console.ReadLine());
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
		}
	}
}