using System.ComponentModel.DataAnnotations;

namespace SharpLab2.DTOs;

public record CreateTicketRequest(
    [Range(1, int.MaxValue)] int PassengerId,
    [Range(1, int.MaxValue)] int TrainId,
    [Range(1, 30)] int CarriageNumber,
    [Range(1, int.MaxValue)] int CarriageTypeId,
    [Required] [MaxLength(20)] string DepartureDate,
    [Range(0, 10000)] decimal UrgencySurcharge
);
