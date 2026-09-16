using System.Data;
using System.Text;
using SharpLab2;

Console.OutputEncoding = Encoding.UTF8;

Database.Initialize();

Console.WriteLine("=== СПИСОК РЕЗЕРВУВАННЯ КВИТКІВ ===");
DataTable tickets = Database.GetTickets();
foreach (DataRow row in tickets.Rows)
{
    Console.WriteLine($"\nКвиток #{row["Id"]}:");
    Console.WriteLine($"1) ПІБ пасажира:\t{row["FullName"]}");
    Console.WriteLine($"2) Домашня адреса:\t{row["Address"]}");
    Console.WriteLine($"3) Телефон:\t\t{row["Phone"]}");
    Console.WriteLine($"4) Номер поїзда:\t{row["TrainNumber"]}");
    Console.WriteLine($"5) Тип поїзда:\t\t{row["TrainType"]}");
    Console.WriteLine($"6) Номер вагона:\t{row["CarriageNumber"]}");
    Console.WriteLine($"7) Тип вагона:\t\t{row["CarriageType"]}");
    Console.WriteLine($"8) Дата відправлення:\t{row["DepartureDate"]}");
    Console.WriteLine($"9) Час відправлення/прибуття:\t{row["DepartureTime"]} / {row["ArrivalTime"]}");
    Console.WriteLine($"10) Пункт призначення:\t{row["Destination"]}");
    Console.WriteLine($"11) Відстань:\t\t{row["DistanceKm"]} км");
    Console.WriteLine($"12) Вартість проїзду:\t{row["BaseFare"]} грн");
    Console.WriteLine($"13) Доплата за терміновість:\t{row["UrgencySurcharge"]} грн");
    Console.WriteLine($"14) Доплата за тип вагона:\t{row["CarriageSurcharge"]} грн");
    Console.WriteLine($"Разом до сплати:\t{row["TotalPrice"]} грн");
}

Console.WriteLine("\n\n=== ПАСАЖИРИ (5 осіб, від 2 бронювань) ===");
DataTable passengers = Database.GetPassengers();
foreach (DataRow row in passengers.Rows)
{
    Console.WriteLine($"- {row["FullName"]} ({row["Phone"]}) — квитків: {row["TicketsCount"]}");
}

Console.WriteLine("\n\n=== ПОЇЗДИ (3 поїзди) ===");
DataTable trains = Database.GetTrains();
foreach (DataRow row in trains.Rows)
{
    Console.WriteLine($"Поїзд #{row["TrainNumber"]} ({row["TrainType"]}) -> {row["Destination"]} ({row["DepartureTime"]} - {row["ArrivalTime"]})");
}

Console.WriteLine("\n\n=== ПУНКТИ ПРИЗНАЧЕННЯ (4 пункти) ===");
DataTable destinations = Database.GetDestinations();
foreach (DataRow row in destinations.Rows)
{
    Console.WriteLine($"- {row["Name"]}: відстань {row["DistanceKm"]} км, базовий тариф {row["BaseFare"]} грн");
}