using System.Runtime.CompilerServices;
using System.Xml.Serialization;

class Programm
{
	static void Main(string[] args)
	{
		while (true)
		{
			Console.WriteLine("Войти как...");
			Console.WriteLine("1. Диспетчер");
			Console.WriteLine("2. Клиент");
			Console.WriteLine("---------------");
			string choice = Console.ReadLine();
			switch (choice)
			{
				case "1":
					Console.WriteLine("Вы вошли как диспетчер.");
					Console.WriteLine("---------------");
					Dispetcher dispetcher = DispatcherReader.ReadDispatcher("./data/input/dispetcherProfile.json");
					List<Order> orders = OrderReader.ReadOrders("./data/input/orders.json");
					List<Auto> cars = AutoParkReader.ReadAutoPark("./data/input/cars.json");
					Autopark autopark = new Autopark(cars, 5, dispetcher);
					Console.WriteLine("Свободные автомобили:\n");
					foreach (Auto a in autopark.Autos)
					{
						Console.WriteLine(a.ToString());
						Console.WriteLine("---------------");
					}

					bool menu = true;
					while (menu)
					{
						Console.WriteLine("\nДоступные действия: ");
						Console.WriteLine("1. Сортировка по расходу топлива");
						Console.WriteLine("2. Подсчитать стоимость таксопарка");
						Console.WriteLine("3. Найти автомобиль, подходящий по скорости");
						Console.WriteLine("4. Просмотреть список заказов");
						Console.WriteLine("0. Выйти из программы.");

						string choice1 = Console.ReadLine();
						switch (choice1)
						{
							case "1":
								for (int i = 0; i < cars.Count - 1; i++)
								{
									for (int j = i + 1; j < cars.Count; j++)
									{
										if (cars[i].FuelConsumption > cars[j].FuelConsumption)
										{
											Auto temp = cars[i];
											cars[i] = cars[j];
											cars[j] = temp;
										}
									}
								}
								foreach (Auto a in autopark.Autos)
								{
									Console.WriteLine(a.ToString());
								}
								break;

							case "2":
								int totalCost = 0;
								foreach (Auto car in cars)
								{
									totalCost += car.Value;
								}

								Console.WriteLine($"Общая стоимость автопарка: {totalCost:N0} руб.");
								break;
							case "3":
								List<Auto> suitableCars = new List<Auto>();
								Console.WriteLine("Введите минимальную скорость:");
								int minSpeed = int.Parse(Console.ReadLine());
								Console.WriteLine("Введите максимальную скорость:");
								int maxSpeed = int.Parse(Console.ReadLine());
								foreach (Auto car in autopark.Autos)
								{
									if (car.MaxSpeed >= minSpeed && car.MaxSpeed >= maxSpeed)
									{
										suitableCars.Add(car);
									}
								}

								if (suitableCars.Count > 0)
								{
									Console.WriteLine($"Автомобили со скоростью от {minSpeed} до {maxSpeed} км/ч:");
									foreach (Auto car in suitableCars)
									{
										Console.WriteLine($"{car.Model} - максимальная скорость {car.MaxSpeed} км/ч");
									}
								}
								else
								{
									Console.WriteLine($"Нет автомобилей со скоростью от {minSpeed} до {maxSpeed} км/ч");
								}
								break;

							case "4":
								foreach (Order o in orders)
								{
									Console.WriteLine(o.ToString());
								}
								break;

							case "0":
								menu = false;
								break;
							default:
								Console.WriteLine("Повторите ввод.");
								break;
						}
					}
					return;
				case "2":
					Console.WriteLine("Вы вошли как клиент.");
					Console.WriteLine("---------------");
					//функция для начала работы и чтения файла
					return;
				default:
					Console.WriteLine("\nНеверный ввод. Попробуйте еще раз.\n");
					break;
			}
		}
	}
}
