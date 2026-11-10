using System.ComponentModel.DataAnnotations;

namespace SharpLab2.DTOs.Passengers;

public record CreatePassengerRequest(
    [Required(ErrorMessage = "Вкажіть ПІБ пасажира.")] [MaxLength(100)] string FullName,
    [Required(ErrorMessage = "Вкажіть адресу пасажира.")] [MaxLength(200)] string Address,
    [Required(ErrorMessage = "Вкажіть номер телефону.")] [MaxLength(20)] string Phone
);
