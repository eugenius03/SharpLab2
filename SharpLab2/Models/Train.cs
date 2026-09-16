using System.ComponentModel.DataAnnotations;

namespace SharpLab2.Models;

public class Train
{
    public int Id { get; init; }

    [MaxLength(20)]
    public string TrainNumber { get; init; } = string.Empty;

    [MaxLength(50)]
    public string TrainType { get; init; } = string.Empty;

    public int DestinationId { get; init; }
    public Destination Destination { get; init; } = null!;

    [MaxLength(10)]
    public string DepartureTime { get; init; } = string.Empty;

    [MaxLength(10)]
    public string ArrivalTime { get; init; } = string.Empty;

    public List<Ticket> Tickets { get; init; } = [];
}
