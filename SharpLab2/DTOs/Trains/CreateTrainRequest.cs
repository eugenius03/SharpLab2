using System.ComponentModel.DataAnnotations;

namespace SharpLab2.DTOs.Trains;

public record CreateTrainRequest(
    [Required(ErrorMessage = "Вкажіть номер потяга.")] [MaxLength(20)] string TrainNumber,
    [Required(ErrorMessage = "Вкажіть тип потяга.")] [MaxLength(50)] string TrainType,
    [Range(1, int.MaxValue, ErrorMessage = "Будь ласка, оберіть станцію призначення зі списку.")] int DestinationId,
    [Required(ErrorMessage = "Вкажіть час відправлення.")] [MaxLength(10)] string DepartureTime,
    [Required(ErrorMessage = "Вкажіть час прибуття.")] [MaxLength(10)] string ArrivalTime
);
