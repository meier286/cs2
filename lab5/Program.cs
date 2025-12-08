public interface IMovable
{
	void SendToClient();

}
public abstract class Auto
{
	public int Value { get; set; }
	public int EnginePower { get; set; }
	public int MaxSpeed { get; set; }
	public int FuelConsumption { get; set; }
	public int CurrentFuel { get; set; }
	public int LuggageSpace { get; set; }
	public string Color { get; set; }
	public bool IsCorrupted { get; set; }
	public int TimeUntilUncorrupted { get; set; }
	public string Model { get; set; }
	public string Brand { get; set; }

	protected Auto(
	int value,
	int engine_power,
	int max_speed,
	int fuel_consumption,
	int current_fuel,
	int luggage_space,
	string color,
	string model,
	string brand)
	{
		this.Value = value;
		this.EnginePower = engine_power;
		this.MaxSpeed = max_speed;
		this.FuelConsumption = fuel_consumption;
		this.CurrentFuel = current_fuel;
		this.LuggageSpace = luggage_space;
		this.Color = color;
		this.IsCorrupted = false;
		this.TimeUntilUncorrupted = -1;
		this.Model = model;
		this.Brand = brand;
	}
}

public abstract class Mercedes : Auto, IMovable
{
	public Client OrderClient { get; set; } = new Client();
	public int ChanceOfGettingBetterRating { get; set; }
	public Mercedes(
	int v,
	int ep,
	int ms,
	int fc,
	int cf,
	int ls,
	string c,
	string m,
	string b,
	int cogbr) : base(v, ep, ms, fc, cf, ls, c, m, b)
	{
		this.ChanceOfGettingBetterRating = cogbr;
	}

	public void SendToClient()
	{

	}
}

public abstract class Volvo : Auto, IMovable
{
	public Client OrderClient { get; set; } = new Client();
	public int AdditionalSeats { get; set; }
	public Volvo(
	int v,
	int ep,
	int ms,
	int fc,
	int cf,
	int ls,
	string c,
	string m,
	string b,
	int ads) : base(v, ep, ms, fc, cf, ls, c, m, b)
	{
		this.AdditionalSeats = ads;
	}

	public override string ToString()
	{
		return "";
	}
	public void SendToClient()
	{
		Console.WriteLine("Автомобиль " + ToString() + "отправлен на вызов.");
		//фокусы автомобиля и результат доставки клиента
	}
}

public abstract class Lada : Auto, IMovable
{
	public Client OrderClient { get; set; } = new Client();
	public int AdditionalLuggageSpace { get; set; }
	public Lada(
	int v,
	int ep,
	int ms,
	int fc,
	int cf,
	int ls,
	string c,
	string m,
	string b,
	int als) : base(v, ep, ms, fc, cf, ls, c, m, b)
	{
		this.AdditionalLuggageSpace = als;
	}
	public void SendToClient()
	{

	}
}

public abstract class Honda : Auto, IMovable
{
	public Client OrderClient { get; set; } = new Client();
	public int ChanceOfFasterComplition { get; set; }
	public Honda(
	int v,
	int ep,
	int ms,
	int fc,
	int cf,
	int ls,
	string c,
	string m,
	string b,
	int cofc) : base(v, ep, ms, fc, cf, ls, c, m, b)
	{
		this.ChanceOfFasterComplition = cofc;
	}
	public void SendToClient()
	{

	}
}

public abstract class Geely : Auto, IMovable
{
	public Client OrderClient { get; set; } = new Client();
	public int ChanceOfBreakDown { get; set; }
	public Geely(
	int v,
	int ep,
	int ms,
	int fc,
	int cf,
	int ls,
	string c,
	string m,
	string b,
	int cobd) : base(v, ep, ms, fc, cf, ls, c, m, b)
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
}

public enum Urgency
{
	Null,
	NonUrgent,
	Urgent,
	VeryUrgent,

}

public class Client
{
	Urgency Urgency { get; set; }
	public int Money { get; set; }
	public int DistanceToTravel { get; set; }
	public int GeneratedRating { get; set; }
	public int Luggage { get; set; }


	public Client()
	{
		this.Urgency = Urgency.Null;
		this.Money = -1;
		this.DistanceToTravel = -1;
		this.Luggage = -1;

	}
	public Client(Urgency u, int m, int dtt, int l)
	{
		this.Urgency = u;
		this.Money = m;
		this.DistanceToTravel = dtt;
		this.Luggage = l;
	}
}

public class Rating
{
	public int NumOfRatings { get; set; }
	public int AverageRating { get; set; }

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


}
//создать таксопарк из файла + сразу же отсортировать машины из файла по расходу топлива
//создать клиентов из файла
//интерактивное меню для выбора какого клиента на какую машину посадить