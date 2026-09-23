using System.ComponentModel.DataAnnotations;

namespace SharpLab2.Models;

public class Ticket : IEntity
{
    public int Id { get; set; }

    public int PassengerId { get; set; }
    public Passenger? Passenger { get; set; }

    public int TrainId { get; set; }
    public Train? Train { get; set; }

    public int CarriageNumber { get; set; }

    public int CarriageTypeId { get; set; }
    public CarriageType? CarriageType { get; set; }

    [Required]
    [MaxLength(20)]
    public string DepartureDate { get; set; } = string.Empty;

    public decimal UrgencySurcharge { get; set; }

    public decimal GetTotalPrice()
    {
        return (Train?.Destination?.BaseFare ?? 0) + (CarriageType?.Surcharge ?? 0) + UrgencySurcharge;
    }
}
