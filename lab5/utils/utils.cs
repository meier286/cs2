using System.Text.Json;
using System.Text.Json.Serialization;

public class OrderReader
{
	private class OrderData
	{
		public string ClientSurname { get; set; } = "";
		public string ClientName { get; set; } = "";
		public string ClientPatronymic { get; set; } = "";
		public int ClientAge { get; set; }
		public int ClientMoney { get; set; }
		public int Baggage { get; set; }
		public int DistanceToTravel { get; set; }
		public int PeopleCount { get; set; }
		public double OrderCost { get; set; }
	}

	public static List<Order> ReadOrders(string ordersFilePath)
	{
		List<Order> orders = new List<Order>();

		if (!File.Exists(ordersFilePath))
		{
			Console.WriteLine("Файл заказов не найден.");
			return orders;
		}

		try
		{
			string jsonText = File.ReadAllText(ordersFilePath);

			if (string.IsNullOrWhiteSpace(jsonText))
			{
				Console.WriteLine("Файл заказов пуст.");
				return orders;
			}

			if (jsonText.Trim() == "[]")
			{
				Console.WriteLine("Заказов нет (пустой массив).");
				return orders;
			}

			List<OrderData> orderDataList = JsonSerializer.Deserialize<List<OrderData>>(jsonText);

			if (orderDataList == null || orderDataList.Count == 0)
			{
				Console.WriteLine("Заказов нет.");
				return orders;
			}

			Console.WriteLine($"Загружено {orderDataList.Count} заказов");

			foreach (var orderData in orderDataList)
			{
				Client client = new Client(
					surname: orderData.ClientSurname,
					name: orderData.ClientName,
					patronymic: orderData.ClientPatronymic,
					age: orderData.ClientAge,
					money: orderData.ClientMoney
				);

				Order order = new Order(
					client: client,
					baggage: orderData.Baggage,
					distanceToTravel: orderData.DistanceToTravel,
					peopleCount: orderData.PeopleCount,
					orderCost: orderData.OrderCost
				);

				orders.Add(order);
			}

			return orders;
		}
		catch (JsonException ex)
		{
			Console.WriteLine($"Ошибка JSON: {ex.Message}");
			return orders;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка: {ex.Message}");
			return orders;
		}
	}
}
public class AutoParkReader
{
	public static List<Auto> ReadAutoPark(string filePath)
	{
		try
		{
			if (!File.Exists(filePath))
			{
				Console.WriteLine($"Файл не найден: {filePath}");
				return new List<Auto>();
			}

			string jsonText = File.ReadAllText(filePath);

			var options = new JsonSerializerOptions
			{
				Converters = { new JsonStringEnumConverter() },
				PropertyNameCaseInsensitive = true
			};

			List<Auto> autos = JsonSerializer.Deserialize<List<Auto>>(jsonText, options);

			if (autos == null)
			{
				Console.WriteLine("Десериализация вернула null");
				return new List<Auto>();
			}

			Console.WriteLine($"Успешно загружено {autos.Count} автомобилей\n");
			return autos;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка при чтении автопарка: {ex.Message}");
			Console.WriteLine($"StackTrace: {ex.StackTrace}");
			return new List<Auto>();

		}
	}
}

public class DispatcherReader
{
	public static Dispetcher ReadDispatcher(string filePath)
	{
		string jsonText = File.ReadAllText(filePath);
		return JsonSerializer.Deserialize<Dispetcher>(jsonText);
	}
}

public class ClientReader
{
	public static Client ReadClient(string filePath)
	{
		if (!File.Exists(filePath))
		{
			Console.WriteLine($"Файл не найден: {filePath}");
			return null;
		}

		try
		{
			string jsonText = File.ReadAllText(filePath);

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};

			Client client = JsonSerializer.Deserialize<Client>(jsonText, options);

			if (client != null)
			{
				Console.WriteLine($"Загружен клиент: {client.Name} {client.Surname}, Возраст: {client.Age}, Деньги: {client.Money}");
			}

			return client;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка чтения клиента: {ex.Message}");
			return null;
		}
	}
}

public class OrderWriter
{
	private class OrderForSave
	{
		public string ClientSurname { get; set; }
		public string ClientName { get; set; }
		public string ClientPatronymic { get; set; }
		public int ClientAge { get; set; }
		public double ClientMoney { get; set; }
		public int Baggage { get; set; }
		public int DistanceToTravel { get; set; }
		public int PeopleCount { get; set; }
		public double OrderCost { get; set; }
	}

	public static void SaveOrder(Order newOrder)
	{
		string filePath = "./data/output/orders.json";

		List<OrderForSave> orders = new List<OrderForSave>();

		if (File.Exists(filePath))
		{
			string jsonText = File.ReadAllText(filePath);
			if (!string.IsNullOrWhiteSpace(jsonText) && jsonText.Trim() != "[]")
			{
				try
				{
					orders = JsonSerializer.Deserialize<List<OrderForSave>>(jsonText) ?? new List<OrderForSave>();
				}
				catch (JsonException)
				{
					orders = new List<OrderForSave>();
				}
			}
		}

		OrderForSave orderToSave = new OrderForSave
		{
			ClientSurname = newOrder.Client.Surname,
			ClientName = newOrder.Client.Name,
			ClientPatronymic = newOrder.Client.Patronymic,
			ClientAge = newOrder.Client.Age,
			ClientMoney = newOrder.Client.Money,
			Baggage = newOrder.Baggage,
			DistanceToTravel = newOrder.DistanceToTravel,
			PeopleCount = newOrder.PeopleCount,
			OrderCost = newOrder.OrderCost
		};

		orders.Add(orderToSave);

		string json = JsonSerializer.Serialize(orders, new JsonSerializerOptions
		{
			WriteIndented = true,
			Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		});

		File.WriteAllText(filePath, json);
		Console.WriteLine("Заказ сохранен!");
	}
}