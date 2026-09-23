using System.ComponentModel.DataAnnotations;

namespace SharpLab2.Models;

public class Passenger : IEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public List<Ticket> Tickets { get; set; } = [];
}
