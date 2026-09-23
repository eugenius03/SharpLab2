namespace SharpLab2.DTOs;

public record TicketResponse(
    int Id,
    int PassengerId,
    string? PassengerName,
    string? PassengerAddress,
    string? PassengerPhone,
    int TrainId,
    string? TrainNumber,
    string? TrainType,
    string? DepartureTime,
    string? ArrivalTime,
    string? DestinationName,
    double DistanceKm,
    decimal BaseFare,
    int CarriageNumber,
    int CarriageTypeId,
    string? CarriageTypeName,
    decimal CarriageSurcharge,
    string DepartureDate,
    decimal UrgencySurcharge,
    decimal TotalPrice
);
