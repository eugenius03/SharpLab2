using System.ComponentModel.DataAnnotations;

namespace SharpLab2.DTOs.Destinations;

public record CreateDestinationRequest(
    [Required(ErrorMessage = "Вкажіть назву станції.")] [MaxLength(100)] string Name,
    [Range(1, 10000, ErrorMessage = "Відстань повинна бути більше 0.")] double DistanceKm,
    [Range(0.01, 100000, ErrorMessage = "Базовий тариф повинен бути більше 0.")] decimal BaseFare
);
