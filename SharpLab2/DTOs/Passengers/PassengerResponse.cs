namespace SharpLab2.DTOs;

public record PassengerResponse(
    int Id,
    string FullName,
    string Address,
    string Phone,
    int TicketsCount
);
