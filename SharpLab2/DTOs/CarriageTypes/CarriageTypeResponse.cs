namespace SharpLab2.DTOs.CarriageTypes;

public record CarriageTypeResponse(
    int Id,
    string TypeName,
    decimal Surcharge,
    int TicketsCount
);
