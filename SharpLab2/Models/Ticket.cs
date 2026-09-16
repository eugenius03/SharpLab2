using System.ComponentModel.DataAnnotations;

namespace SharpLab2.Models;

public class Ticket
{
    public int Id { get; init; }
    public int PassengerId { get; init; }
    public Passenger Passenger { get; init; } = null!;
    public int TrainId { get; init; }
    public Train Train { get; init; } = null!;
    public int CarriageNumber { get; init; }
    public int CarriageTypeId { get; init; }
    public CarriageType CarriageType { get; init; } = null!;

    [MaxLength(20)]
    public string DepartureDate { get; init; } = string.Empty;

    public decimal UrgencySurcharge { get; init; }

    public decimal GetTotalPrice()
    {
        return Train.Destination.BaseFare + CarriageType.Surcharge + UrgencySurcharge;
    }
}
