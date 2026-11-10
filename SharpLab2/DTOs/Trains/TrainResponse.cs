namespace SharpLab2.DTOs.Trains;

public record TrainResponse(
    int Id,
    string TrainNumber,
    string TrainType,
    int DestinationId,
    string? DestinationName,
    string DepartureTime,
    string ArrivalTime,
    int TicketsCount
);
