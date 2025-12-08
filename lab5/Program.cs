public interface IHumanable
{
	void GenerateRating();
}

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
	public int ChanceOfGettingBetterRating { get; set; }
	Mercedes(
	int v,
	int ep,
	int ms,
	int fc,
	int cf,
	int ls,
	string c,
	bool ic,
	int tuu,
	string m,
	string b,
	int cogbr) : base(v, ep, ms, fc, cf, ls, c, ic, tuu, m, b)
	{
		this.ChanceOfGettingBetterRating = cogbr;
	}

	public void SendToClient()
	{

	}
}

public abstract class Volvo : Auto, IMovable
{
	public int AdditionalSeats { get; set; }
	Volvo(int v,
	int ep,
	int ms,
	int fc,
	int cf,
	int ls,
	string c,
	bool ic,
	int tuu,
	string m,
	string b,
	int ads) : base(v, ep, ms, fc, cf, ls, c, ic, tuu, m, b)
	{
		this.AdditionalSeats = ads;
	}
	public void SendToClient()
	{

	}
}

public abstract class Lada : Auto, IMovable
{
	public int AdditionalLuggageSpace { get; set; }
	Lada(int v,
	int ep,
	int ms,
	int fc,
	int cf,
	int ls,
	string c,
	bool ic,
	int tuu,
	string m,
	string b,
	int als) : base(v, ep, ms, fc, cf, ls, c, ic, tuu, m, b)
	{
		this.AdditionalLuggageSpace = als;
	}
	public void SendToClient()
	{

	}
}

public abstract class Honda : Auto, IMovable
{
	public int ChanceOfFasterComplition { get; set; }
	Honda(int v,
	int ep,
	int ms,
	int fc,
	int cf,
	int ls,
	string c,
	bool ic,
	int tuu,
	string m,
	string b,
	int cofc) : base(v, ep, ms, fc, cf, ls, c, ic, tuu, m, b)
	{
		this.ChanceOfFasterComplition = cofc;
	}
	public void SendToClient()
	{

	}
}

public abstract class Geely : Auto, IMovable
{
	public int ChanceOfBreakDown { get; set; }
	Geely(int v,
	int ep,
	int ms,
	int fc,
	int cf,
	int ls,
	string c,
	bool ic,
	int tuu,
	string m,
	string b,
	int cobd) : base(v, ep, ms, fc, cf, ls, c, ic, tuu, m, b)
	{
		this.ChanceOfBreakDown = cobd;
	}

	public bool Repair()
	{
		//сколько стоило
		return true;
	}
	public void SendToClient()
	{

	}
}

public enum Urgency
{
	NonUrgent,
	Urgent,
	VeryUrgent,

}

public class Client : IHumanable
{
	Urgency Urgency { get; set; }
	public int Money { get; set; }
	public int DistanceToTravel { get; set; }
	public int GeneratedRating { get; set; }
	public int Luggage { get; set; }
}

public struct Rating
{
	public int NumOfRatings { get; set; }
	public int AverageRating { get; set; }

	Rating()
	{
		this.NumOfRatings = 0;
		this.AverageRating = 0;
	}
}
public class TaxiCompany()
{
	public int Balance { get; set; }
	public Rating rating { get; set; } = Rating();





}
//создать таксопарк из файла + сразу же отсортировать машины из файла по расходу топлива
//создать клиентов из файла
//интерактивное меню для выбора какого клиента на какую машину посадить