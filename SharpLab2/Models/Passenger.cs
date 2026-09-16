using System.ComponentModel.DataAnnotations;

namespace SharpLab2.Models;

public class Passenger
{
    public int Id { get; init; }

    [MaxLength(100)]
    public string FullName { get; init; } = string.Empty;

    [MaxLength(200)]
    public string Address { get; init; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; init; } = string.Empty;

    public List<Ticket> Tickets { get; init; } = [];
}
