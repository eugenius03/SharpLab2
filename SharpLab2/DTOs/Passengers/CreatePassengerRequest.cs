using System.ComponentModel.DataAnnotations;

namespace SharpLab2.DTOs;

public record CreatePassengerRequest(
    [Required] [MaxLength(100)] string FullName,
    [Required] [MaxLength(200)] string Address,
    [Required] [MaxLength(20)] string Phone
);
