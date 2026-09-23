using System.ComponentModel.DataAnnotations;

namespace SharpLab2.Models;

public class CarriageType : IEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string TypeName { get; set; } = string.Empty;

    public decimal Surcharge { get; set; }

    public List<Ticket> Tickets { get; set; } = [];
}
