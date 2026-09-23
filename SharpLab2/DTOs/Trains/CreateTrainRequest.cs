using System.ComponentModel.DataAnnotations;

namespace SharpLab2.DTOs;

public record CreateTrainRequest(
    [Required] [MaxLength(20)] string TrainNumber,
    [Required] [MaxLength(50)] string TrainType,
    [Range(1, int.MaxValue)] int DestinationId,
    [Required] [MaxLength(10)] string DepartureTime,
    [Required] [MaxLength(10)] string ArrivalTime
);
