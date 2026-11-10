using System.ComponentModel.DataAnnotations;

namespace SharpLab2.DTOs.Tickets;

public record CreateTicketRequest(
    [Range(1, int.MaxValue, ErrorMessage = "Будь ласка, оберіть пасажира зі списку.")] int PassengerId,
    [Range(1, int.MaxValue, ErrorMessage = "Будь ласка, оберіть потяг зі списку.")] int TrainId,
    [Range(1, 30, ErrorMessage = "Номер вагона повинен бути в межах від 1 до 30.")] int CarriageNumber,
    [Range(1, int.MaxValue, ErrorMessage = "Будь ласка, оберіть тип вагона зі списку.")] int CarriageTypeId,
    [Required(ErrorMessage = "Вкажіть дату відправлення.")] [MaxLength(20)] string DepartureDate,
    [Range(0, 10000, ErrorMessage = "Надбавка за терміновість не може бути від'ємною.")] decimal UrgencySurcharge
);
