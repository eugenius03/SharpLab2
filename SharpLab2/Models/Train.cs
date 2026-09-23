using System.ComponentModel.DataAnnotations;

namespace SharpLab2.Models;

public class Train : IEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string TrainNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TrainType { get; set; } = string.Empty;

    public int DestinationId { get; set; }
    public Destination? Destination { get; set; }

    [Required]
    [MaxLength(10)]
    public string DepartureTime { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string ArrivalTime { get; set; } = string.Empty;

    public List<Ticket> Tickets { get; set; } = [];
}
