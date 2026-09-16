using System.ComponentModel.DataAnnotations;

namespace SharpLab2.Models;

public class Destination
{
    public int Id { get; init; }

    [MaxLength(100)]
    public string Name { get; init; } = string.Empty;

    public double DistanceKm { get; init; }
    public decimal BaseFare { get; init; }

    public List<Train> Trains { get; init; } = [];
}
