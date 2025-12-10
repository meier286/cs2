using System.Data.Common;
using System.Diagnostics.Contracts;
using System.IO.Pipes;

public interface IMovable
{
	void SendToClient();
	int UsefullAbility();

}
public abstract class Auto
{
	public int Value { get; set; }
	public int EnginePower { get; set; }
	public int MaxSpeed { get; set; }
	public int FuelConsumption { get; set; }
	public int CurrentFuel { get; set; }
	public int LuggageSpace { get; set; }
	public int Seats { get; set; }
	public string Color { get; set; }
	public bool IsCorrupted { get; set; }
	public int TimeUntilUncorrupted { get; set; }
	public string Model { get; set; }

	protected Auto(
	int value,
	int engine_power,
	int max_speed,
	int fuel_consumption,
	int current_fuel,
	int luggage_space,
	int seats,
	string color,
	string model)
	{
		this.Value = value;
		this.EnginePower = engine_power;
		this.MaxSpeed = max_speed;
		this.FuelConsumption = fuel_consumption;
		this.CurrentFuel = current_fuel;
		this.LuggageSpace = luggage_space;
		this.Seats = seats;
		this.Color = color;
		this.IsCorrupted = false;
		this.TimeUntilUncorrupted = -1;
		this.Model = model;
	}

	public int CalcutaleMaxDistance()
	{
		return CurrentFuel / FuelConsumption;
	}

	public override string ToString()
	{
		return "Автомобиль " + Model +
		"\nХарактеристики:" +
		"\nМощность двигателя: " + EnginePower +
		"\nМаксимальная скорость: " + MaxSpeed +
		"\nРасход топлива: " + FuelConsumption +
		"\nМесто для багажа: " + LuggageSpace +
		"\nЦвет: " + Color;
	}
}

public abstract class Mercedes : Auto, IMovable
{
	public TaxiCompany ownedByTaxiCompany { get; set; } = new TaxiCompany();
	public Client OrderClient { get; set; } = new Client();
	//затем прочитать из файла и заменить
	public int ChanceOfGettingBetterRating { get; set; }
	public Mercedes(
	int v,
	int ep,
	int ms,
	int fc,
	int cf,
	int ls,
	int s,
	string c,
	string b,
	int cogbr) : base(v, ep, ms, fc, cf, ls, s, c, b)
	{
		this.ChanceOfGettingBetterRating = cogbr;
	}

	public override string ToString()
	{
		return base.ToString() + "\nШанс получить более высокую оценку: " + ChanceOfGettingBetterRating;
	}
	public int UsefullAbility()
	{
		Random random = new Random();
		int satisfiedNum = ChanceOfGettingBetterRating * 5;
		int randNum = random.Next(0, 5);
		int isAbilityUsed = 0;
		if (randNum < satisfiedNum)
		{
			isAbilityUsed = 1;
			return isAbilityUsed;
		}
		return isAbilityUsed;

	}
	public void SendToClient()
	{
		IsCorrupted = true;
		int isAbilityUsed = UsefullAbility();
		if (isAbilityUsed == 0)
		{
			Random random = new Random();
			int min = 3;
			int max = 5;
			OrderClient.GeneratedRating = random.Next(min, max);
			int AvailableDistanceToTravel = CalcutaleMaxDistance();
			if (OrderClient.Luggage > LuggageSpace)
			{
				OrderClient.GeneratedRating -= 2;
			}

			if (OrderClient.DistanceToTravel > AvailableDistanceToTravel || OrderClient.Humans > Seats)
			{
				OrderClient.GeneratedRating = 1;
			}

		}

		else
		{
			OrderClient.GeneratedRating = 5;
		}

		ownedByTaxiCompany.Balance += OrderClient.Money;
		ownedByTaxiCompany.Rating.AverageRating = (ownedByTaxiCompany.Rating.AverageRating * ownedByTaxiCompany.Rating.NumOfRatings + OrderClient.GeneratedRating) / (ownedByTaxiCompany.Rating.NumOfRatings + 1);
		ownedByTaxiCompany.Rating.NumOfRatings++;

	}

	public abstract class Volvo : Auto, IMovable
	{
		public TaxiCompany ownedByTaxiCompany { get; set; } = new TaxiCompany();
		public Client OrderClient { get; set; } = new Client();
		public int ExtraSeatsChance { get; set; }
		public Volvo(
		int v,
		int ep,
		int ms,
		int fc,
		int cf,
		int ls,
		int s,
		string c,
		string b,
		int esc) : base(v, ep, ms, fc, cf, ls, s, c, b)
		{
			this.ExtraSeatsChance = esc;
		}

		public override string ToString()
		{
			return "\nШанс, что все клиенты втеснятся в автомобиль: " + ExtraSeatsChance;
		}
		public void SendToClient()
		{
			Console.WriteLine("Автомобиль " + ToString() + "отправлен на вызов.");
			//фокусы автомобиля и результат доставки клиента
		}

		public int UsefullAbility()
		{
			return 1;
		}
	}

	public abstract class Lada : Auto, IMovable
	{
		public TaxiCompany ownedByTaxiCompany { get; set; } = new TaxiCompany();
		public Client OrderClient { get; set; } = new Client();
		public int AdditionalLuggageSpace { get; set; }
		public Lada(
		int v,
		int ep,
		int ms,
		int fc,
		int cf,
		int ls,
		int s,
		string c,
		string b,
		int als) : base(v, ep, ms, fc, cf, ls, s, c, b)
		{
			this.AdditionalLuggageSpace = als;
		}
		public void SendToClient()
		{

		}

		public int UsefullAbility()
		{
			return 1;
		}
	}

	public abstract class Honda : Auto, IMovable
	{
		public TaxiCompany ownedByTaxiCompany { get; set; } = new TaxiCompany();

		public Client OrderClient { get; set; } = new Client();
		public int ChanceOfFasterComplition { get; set; }
		public Honda(
		int v,
		int ep,
		int ms,
		int fc,
		int cf,
		int ls,
		int s,
		string c,
		string b,
		int cofc) : base(v, ep, ms, fc, cf, ls, s, c, b)
		{
			this.ChanceOfFasterComplition = cofc;
		}
		public void SendToClient()
		{

		}

		public int UsefullAbility()
		{
			return 1;
		}
	}

	public abstract class Geely : Auto, IMovable
	{
		public TaxiCompany ownedByTaxiCompany { get; set; } = new TaxiCompany();
		public Client OrderClient { get; set; } = new Client();
		public int ChanceOfBreakDown { get; set; }
		public Geely(
		int v,
		int ep,
		int ms,
		int fc,
		int cf,
		int ls,
		int s,
		string c,
		string b,
		int cobd) : base(v, ep, ms, fc, cf, ls, s, c, b)
		{
			this.ChanceOfBreakDown = cobd;
		}

		public bool TryToRepair(Random r)
		{
			int max = 5;
			int repairRandom = r.Next(max);

			bool isRepaired;
			if (repairRandom <= 2)
			{
				isRepaired = false;
			}
			else
			{
				isRepaired = true;
			}

			return isRepaired;


		}
		public bool Repair()
		{
			Random random = new Random();
			int min = 500;
			int max = 25000;
			int repairValue = random.Next(min, max);

			Console.WriteLine("Автомобиль сломался. Клиент недоволен. Стоимость ремонта: " + repairValue + ". Починить автомобиль? (Y/n)");
			string choice = Console.ReadLine();
			switch (choice)
			{
				case "n":
					Console.WriteLine("Автомобиль требует ремонта. Клиент оставил негативный отзыв.");
					return false;
				default:
					bool isRepaired = TryToRepair(random);
					Console.WriteLine("Текущий бюджет:");
					return isRepaired;
			}
		}
		public void SendToClient()
		{

		}

		public int UsefullAbility()
		{
			return 1;
		}
	}

	public class Client
	{
		public int Humans { get; set; }
		public int Money { get; set; }
		public int DistanceToTravel { get; set; }
		public int GeneratedRating { get; set; }
		public int Luggage { get; set; }


		public Client()
		{
			this.Humans = -1;
			this.Money = -1;
			this.DistanceToTravel = -1;
			this.Luggage = -1;

		}
		public Client(int h, int m, int dtt, int l)
		{
			this.Humans = h;
			this.Money = m;
			this.DistanceToTravel = dtt;
			this.Luggage = l;
		}
	}

	public class Rating
	{
		public int NumOfRatings { get; set; }
		public double AverageRating { get; set; }

		public Rating()
		{
			this.NumOfRatings = 0;
			this.AverageRating = 0;
		}
	}
	public class TaxiCompany
	{
		public int Balance { get; set; }
		public Rating Rating { get; set; } = new Rating();

		public TaxiCompany(int b, int nor, int ar)
		{
			this.Balance = b;
			this.Rating.NumOfRatings = nor;
			this.Rating.AverageRating = ar;
		}

		public TaxiCompany()
		{
			this.Balance = -1;
			this.Rating.NumOfRatings = -1;
			this.Rating.AverageRating = -1;
		}


	}
}
//создать таксопарк из файла + сразу же отсортировать машины из файла по расходу топлива
//создать клиентов из файла
//интерактивное меню для выбора какого клиента на какую машину посадить