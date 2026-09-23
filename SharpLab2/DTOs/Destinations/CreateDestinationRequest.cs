using System.ComponentModel.DataAnnotations;

namespace SharpLab2.DTOs;

public record CreateDestinationRequest(
    [Required] [MaxLength(100)] string Name,
    [Range(0, 10000)] double DistanceKm,
    [Range(0, 100000)] decimal BaseFare
);
