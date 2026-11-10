namespace SharpLab2.DTOs.Passengers;

public record PassengerResponse(
    int Id,
    string FullName,
    string Address,
    string Phone,
    int TicketsCount
);
