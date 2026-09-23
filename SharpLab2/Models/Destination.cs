using System.ComponentModel.DataAnnotations;

namespace SharpLab2.Models;

public class Destination : IEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public double DistanceKm { get; set; }
    public decimal BaseFare { get; set; }

    public List<Train> Trains { get; set; } = [];
}
