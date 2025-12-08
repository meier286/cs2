public interface IMovable
{
	string ToString();
	void SendToClient();

}
public abstract class Auto
{
	public int Value { get; set; }
	public int EnginePower { get; set; }
	public int MaxSpeed { get; set; }
	public int FuelConsumption { get; set; }
	public int LuggageSpace { get; set; }
	public string Model { get; set; }
	public string Color { get; set; }

	public bool IsCorrupted { get; set; }

	public int TimeUntilUncorrupted { get; set; }
	public string Brand { get; set; }

	protected Auto(int value, int engine_power, int max_speed, int fuel_consumption, string model, string color)
	{
		this.Value = value;
		this.EnginePower = engine_power;
		this.max_speed = max_speed;
		this.fuel_consumption = fuel_consumption;
		this.model = model;
		this.color = color;
	}
}

public abstract class Mercedes : Auto
{
	public int ChanceOfGettingBetterRating { get; set; }
	Mercedes(int v, int e, int s, int f, string m, string c) : base(v, e, s, f, m, c)
	{

	}
}

public abstract class Volvo : Auto
{
	public int AdditionalSeats { get; set; }
	Volvo() : base()
	{

	}
}

public abstract class Lada : Auto
{
	public int AdditionalLuggageSpace { get; set; }
	Lada() : base()
	{

	}
}

public abstract class Honda : Auto
{
	public int ChanceOfFasterComplition { get; set; }
	Honda() : base()
	{

	}
}

public abstract class Geely : Auto
{
	public int ChanceOfBreakDown { get; set; }
	Geely(int v, int e, int s, int f, string m, string c) : base(v, e, s, f, m, c)
	{

	}
}

public enum Urgency
{
	NonUrgent,
	Urgent,
	VeryUrgent,

}

public class Client
{
	public int Money { get; set; }
	public int GeneratedRating { get; set; }
	public int Luggage { get; set; }
}