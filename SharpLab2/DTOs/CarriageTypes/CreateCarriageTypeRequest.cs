using System.ComponentModel.DataAnnotations;

namespace SharpLab2.DTOs;

public record CreateCarriageTypeRequest(
    [Required] [MaxLength(50)] string TypeName,
    [Range(0, 10000)] decimal Surcharge
);
