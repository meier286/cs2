using System.Text.Json;
using System.Text.Json.Serialization;
public class OrderReader
{
	private class OrderData
	{
		public string ClientSurname { get; set; }
		public string ClientName { get; set; }
		public string ClientPatronymic { get; set; }
		public int ClientAge { get; set; }
		public int ClientMoney { get; set; }
		public int Baggage { get; set; }
		public int DistanceToTravel { get; set; }
		public int PeopleCount { get; set; }
		public int OrderCost { get; set; }
	}

	public static List<Order> ReadOrders(string ordersFilePath)
	{
		List<Order> orders = new List<Order>();

		if (!File.Exists(ordersFilePath))
		{
			throw new FileNotFoundException($"Файл заказов не найден: {ordersFilePath}");
		}

		try
		{
			string jsonText = File.ReadAllText(ordersFilePath);
			List<OrderData> orderDataList = JsonSerializer.Deserialize<List<OrderData>>(jsonText);

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
			throw new ArgumentException($"Ошибка при чтении JSON файла: {ex.Message}");
		}
		catch (Exception ex)
		{
			throw new Exception($"Ошибка при чтении файла заказов: {ex.Message}");
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