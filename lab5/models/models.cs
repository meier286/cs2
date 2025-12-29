using System.Data.Common;
using System.Diagnostics.Contracts;
using System.IO.Pipes;
using System.Reflection.PortableExecutable;

public enum Body
{
	Minivan,
	Crossover,
	Universal,
	Hatchback,
	Sedan,
	Coupe
}

public interface IMovable
{
	void SendToClient();

}

public abstract class Human
{
	public string Surname { get; set; }
	public string Name { get; set; }
	public string Patronymic { get; set; }
	public Human(string surname, string name, string patronymic)
	{
		this.Surname = surname;
		this.Name = name;
		this.Patronymic = patronymic;
	}
}
public class Auto : IMovable
{
	public int Value { get; set; }
	public Body Body { get; set; }
	public int EnginePower { get; set; }
	public int MaxSpeed { get; set; }
	public int FuelConsumption { get; set; }
	public int MaxFuel { get; set; }
	public int CurrentFuel { get; set; }
	public int LuggageSpace { get; set; }
	public int Seats { get; set; }
	public string Color { get; set; }
	public bool IsCorrupted { get; set; }
	public string Model { get; set; }


	public Auto()
	{

	}
	public Auto(
	int value,
	Body body,
	int engine_power,
	int max_speed,
	int fuel_consumption,
	int max_fuel,
	int current_fuel,
	int luggage_space,
	int seats,
	string color,
	string model)
	{
		this.Value = value;
		this.Body = body;
		this.EnginePower = engine_power;
		this.MaxSpeed = max_speed;
		this.FuelConsumption = fuel_consumption;
		this.MaxFuel = max_fuel;
		this.CurrentFuel = current_fuel;
		this.LuggageSpace = luggage_space;
		this.Seats = seats;
		this.Color = color;
		this.IsCorrupted = false;
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
		"\nМощность двигателя: " + EnginePower + " л.с." +
		"\nКузов: " + Body +
		"\nМаксимальная скорость: " + MaxSpeed + " км/ч" +
		"\nРасход топлива: " + FuelConsumption + " л/100км" +
		"\nМеста для пассажиров: " + Seats +
		"\nМесто для багажа: " + LuggageSpace + " л" +
		"\nЦвет: " + Color;
	}

	public void SendToClient()
	{
		Console.WriteLine("Автомобиль отправлен клиенту.");
		//еще какаято логика
	}
}

public class Dispetcher : Human
{
	public int OrdersCount { get; set; }

	public Dispetcher(string surname, string name, string patronymic, int ordersCount) : base(surname, name, patronymic)
	{
		this.OrdersCount = ordersCount;
	}

}

public class Client : Human
{
	public int Age;
	public int Money;
	public Client(string surname, string name, string patronymic, int age, int money) : base(surname, name, patronymic)
	{
		this.Age = age;
		this.Money = money;
	}

	public void WriteMark()
	{

	}
}

public class Order
{
	public Client Client { get; set; }
	public int Baggage { get; set; }
	public int DistanceToTravel { get; set; }
	public int PeopleCount { get; set; }
	public int OrderCost { get; set; }

	public Order(Client client, int baggage, int distanceToTravel, int peopleCount, int orderCost)
	{

		this.Client = client;
		this.Baggage = baggage;
		this.DistanceToTravel = distanceToTravel;
		this.PeopleCount = peopleCount;
		this.OrderCost = orderCost;
	}

	public override string ToString()
	{
		return
		"Фамилия клиента " + Client.Surname +
		"\nИмя клиента: " + Client.Name +
		"\nОтчество клиента " + Client.Patronymic +
		"\nОбъем багажа: " + Baggage + " л" +
		"\nРасстояние до пункта назначения: " + DistanceToTravel + " км" +
		"\nКоличество пассажиров " + PeopleCount +
		"\nСтоимость заказа " + OrderCost + "\n";
	}

}

public class Autopark
{
	public int Rating;
	public List<Auto> Autos;
	public Dispetcher CurrentDispetcher;
	public Autopark(List<Auto> autos, int rating, Dispetcher dispetcher)
	{
		this.Autos = autos;
		this.Rating = rating;
		this.CurrentDispetcher = dispetcher;
	}
}

//создать таксопарк из файла + сразу же отсортировать машины из файла по расходу топлива
//создать клиентов из файла
//интерактивное меню для выбора какого клиента на какую машину посадить