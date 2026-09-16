using System.Text;
using SharpLab2.Data;
using SharpLab2.Services;

Console.OutputEncoding = Encoding.UTF8;

using var context = new AppDbContext();

var databaseInitializer = new DatabaseInitializer(context);
databaseInitializer.Initialize();

var ticketService = new TicketService(context);

Console.WriteLine("=== СПИСОК РЕЗЕРВУВАННЯ КВИТКІВ ===");
var tickets = ticketService.GetAllTickets();
foreach (var t in tickets)
{
    Console.WriteLine($"\nКвиток #{t.Id}:");
    Console.WriteLine($"1) ПІБ пасажира:\t{t.Passenger.FullName}");
    Console.WriteLine($"2) Домашня адреса:\t{t.Passenger.Address}");
    Console.WriteLine($"3) Телефон:\t\t{t.Passenger.Phone}");
    Console.WriteLine($"4) Номер поїзда:\t{t.Train.TrainNumber}");
    Console.WriteLine($"5) Тип поїзда:\t\t{t.Train.TrainType}");
    Console.WriteLine($"6) Номер вагона:\t{t.CarriageNumber}");
    Console.WriteLine($"7) Тип вагона:\t\t{t.CarriageType.TypeName}");
    Console.WriteLine($"8) Дата відправлення:\t{t.DepartureDate}");
    Console.WriteLine($"9) Час відправлення/прибуття:\t{t.Train.DepartureTime} / {t.Train.ArrivalTime}");
    Console.WriteLine($"10) Пункт призначення:\t{t.Train.Destination.Name}");
    Console.WriteLine($"11) Відстань:\t\t{t.Train.Destination.DistanceKm} км");
    Console.WriteLine($"12) Вартість проїзду:\t{t.Train.Destination.BaseFare} грн");
    Console.WriteLine($"13) Доплата за терміновість:\t{t.UrgencySurcharge} грн");
    Console.WriteLine($"14) Доплата за тип вагона:\t{t.CarriageType.Surcharge} грн");
    Console.WriteLine($"Разом до сплати:\t{t.GetTotalPrice()} грн");
}

Console.WriteLine("\n\n=== ПАСАЖИРИ (5 осіб, від 2 бронювань) ===");
var passengers = ticketService.GetAllPassengers();
foreach (var p in passengers)
{
    Console.WriteLine($"- {p.FullName} ({p.Phone}) — квитків: {p.Tickets.Count}");
}

Console.WriteLine("\n\n=== ПОЇЗДИ (3 поїзди) ===");
var trains = ticketService.GetAllTrains();
foreach (var tr in trains)
{
    Console.WriteLine($"Поїзд #{tr.TrainNumber} ({tr.TrainType}) -> {tr.Destination.Name} ({tr.DepartureTime} - {tr.ArrivalTime})");
}

Console.WriteLine("\n\n=== ПУНКТИ ПРИЗНАЧЕННЯ (4 пункти) ===");
var destinations = ticketService.GetAllDestinations();
foreach (var d in destinations)
{
    Console.WriteLine($"- {d.Name}: відстань {d.DistanceKm} км, базовий тариф {d.BaseFare} грн");
}