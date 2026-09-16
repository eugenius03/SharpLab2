using System.ComponentModel.DataAnnotations;

namespace SharpLab2.Models;

public class CarriageType
{
    public int Id { get; init; }

    [MaxLength(50)]
    public string TypeName { get; init; } = string.Empty;

    public decimal Surcharge { get; init; }

    public List<Ticket> Tickets { get; init; } = [];
}
