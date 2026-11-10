namespace SharpLab2.DTOs.Destinations;

public record DestinationResponse(
    int Id,
    string Name,
    double DistanceKm,
    decimal BaseFare,
    int TrainsCount
);
