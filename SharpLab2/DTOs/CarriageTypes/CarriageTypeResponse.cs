namespace SharpLab2.DTOs;

public record CarriageTypeResponse(
    int Id,
    string TypeName,
    decimal Surcharge,
    int TicketsCount
);
