using System.Data;
using Microsoft.Data.Sqlite;

namespace SharpLab2;

public static class Database
{
    private const string ConnectionString = "Data Source=tickets.db";

    public static void Initialize()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            PRAGMA foreign_keys = ON;

            CREATE TABLE IF NOT EXISTS Destinations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                DistanceKm REAL NOT NULL,
                BaseFare REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS CarriageTypes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TypeName TEXT NOT NULL,
                Surcharge REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Passengers (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT NOT NULL,
                Address TEXT NOT NULL,
                Phone TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Trains (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TrainNumber TEXT NOT NULL,
                TrainType TEXT NOT NULL,
                DestinationId INTEGER NOT NULL,
                DepartureTime TEXT NOT NULL,
                ArrivalTime TEXT NOT NULL,
                FOREIGN KEY (DestinationId) REFERENCES Destinations(Id)
            );

            CREATE TABLE IF NOT EXISTS Tickets (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PassengerId INTEGER NOT NULL,
                TrainId INTEGER NOT NULL,
                CarriageNumber INTEGER NOT NULL,
                CarriageTypeId INTEGER NOT NULL,
                DepartureDate TEXT NOT NULL,
                UrgencySurcharge REAL NOT NULL DEFAULT 0,
                FOREIGN KEY (PassengerId) REFERENCES Passengers(Id),
                FOREIGN KEY (TrainId) REFERENCES Trains(Id),
                FOREIGN KEY (CarriageTypeId) REFERENCES CarriageTypes(Id)
            );
        ";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "SELECT COUNT(*) FROM Passengers;";
        long count = (long)(cmd.ExecuteScalar() ?? 0L);

        if (count == 0)
        {
            SeedData(connection);
        }
    }

    private static void SeedData(SqliteConnection connection)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Destinations (Name, DistanceKm, BaseFare) VALUES
            ('Львів', 267, 240.00),
            ('Тернопіль', 180, 180.00),
            ('Хмельницький', 210, 200.00),
            ('Вінниця', 330, 280.00);

            INSERT INTO CarriageTypes (TypeName, Surcharge) VALUES
            ('Загальний', 0.00),
            ('Плацкартний', 70.00),
            ('Купе', 160.00),
            ('Люкс', 400.00);

            INSERT INTO Passengers (FullName, Address, Phone) VALUES
            ('Коваленко Іван Петрович', 'м. Чернівці, вул. Головна, 45', '+380501112233'),
            ('Мельник Олена Василівна', 'м. Чернівці, вул. Героїв Майдану, 12', '+380672223344'),
            ('Бондаренко Сергій Миколайович', 'м. Чернівці, вул. Університетська, 8', '+380933334455'),
            ('Ткаченко Марія Іванівна', 'м. Чернівці, вул. Руська, 103', '+380504445566'),
            ('Кравченко Андрій Володимирович', 'м. Чернівці, вул. Кобилянської, 22', '+380995556677');

            INSERT INTO Trains (TrainNumber, TrainType, DestinationId, DepartureTime, ArrivalTime) VALUES
            ('702О', 'Інтерсіті', 1, '06:00', '09:45'),
            ('118Ш', 'Швидкісний', 4, '19:45', '04:20'),
            ('358Л', 'Пасажирський', 2, '08:20', '12:40');

            INSERT INTO Tickets (PassengerId, TrainId, CarriageNumber, CarriageTypeId, DepartureDate, UrgencySurcharge) VALUES
            (1, 1, 2, 3, '2026-09-20', 0.00),
            (1, 2, 1, 4, '2026-09-25', 50.00),
            (2, 2, 2, 3, '2026-09-22', 0.00),
            (2, 3, 2, 3, '2026-10-02', 30.00),
            (3, 1, 1, 4, '2026-09-21', 80.00),
            (3, 2, 2, 3, '2026-09-28', 0.00),
            (3, 3, 1, 3, '2026-10-05', 0.00),
            (4, 3, 1, 3, '2026-09-24', 40.00),
            (4, 1, 3, 2, '2026-10-01', 0.00),
            (5, 2, 3, 2, '2026-09-23', 0.00),
            (5, 1, 1, 4, '2026-09-29', 90.00);
        ";
        cmd.ExecuteNonQuery();
    }

    public static DataTable GetTickets()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        string sql = @"
            SELECT 
                t.Id,
                p.FullName,
                p.Address,
                p.Phone,
                tr.TrainNumber,
                tr.TrainType,
                t.CarriageNumber,
                ct.TypeName AS CarriageType,
                t.DepartureDate,
                tr.DepartureTime,
                tr.ArrivalTime,
                d.Name AS Destination,
                d.DistanceKm,
                d.BaseFare,
                t.UrgencySurcharge,
                ct.Surcharge AS CarriageSurcharge,
                (d.BaseFare + ct.Surcharge + t.UrgencySurcharge) AS TotalPrice
            FROM Tickets t
            JOIN Passengers p ON t.PassengerId = p.Id
            JOIN Trains tr ON t.TrainId = tr.Id
            JOIN Destinations d ON tr.DestinationId = d.Id
            JOIN CarriageTypes ct ON t.CarriageTypeId = ct.Id
            ORDER BY t.Id;
        ";

        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        var table = new DataTable();
        table.Load(reader);
        return table;
    }

    public static DataTable GetPassengers()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        string sql = @"
            SELECT 
                p.FullName,
                p.Phone,
                COUNT(t.Id) AS TicketsCount
            FROM Passengers p
            LEFT JOIN Tickets t ON p.Id = t.PassengerId
            GROUP BY p.Id
            ORDER BY p.Id;
        ";

        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        var table = new DataTable();
        table.Load(reader);
        return table;
    }

    public static DataTable GetTrains()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        string sql = @"
            SELECT 
                tr.TrainNumber,
                tr.TrainType,
                d.Name AS Destination,
                tr.DepartureTime,
                tr.ArrivalTime
            FROM Trains tr
            JOIN Destinations d ON tr.DestinationId = d.Id
            ORDER BY tr.Id;
        ";

        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        var table = new DataTable();
        table.Load(reader);
        return table;
    }

    public static DataTable GetDestinations()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        string sql = "SELECT Name, DistanceKm, BaseFare FROM Destinations ORDER BY Id;";

        using var cmd = new SqliteCommand(sql, connection);
        using var reader = cmd.ExecuteReader();
        var table = new DataTable();
        table.Load(reader);
        return table;
    }
}
