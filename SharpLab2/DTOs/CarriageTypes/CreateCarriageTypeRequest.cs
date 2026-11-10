using System.ComponentModel.DataAnnotations;

namespace SharpLab2.DTOs.CarriageTypes;

public record CreateCarriageTypeRequest(
    [Required(ErrorMessage = "Вкажіть назву типу вагона.")] [MaxLength(50)] string TypeName,
    [Range(0, 100000, ErrorMessage = "Надбавка не може бути від'ємною.")] decimal Surcharge
);
