using Microsoft.EntityFrameworkCore;
using SharpLab2.Data;
using SharpLab2.Models;

namespace SharpLab2.Services;

public class TicketService(AppDbContext context)
{
    public List<Ticket> GetAllTickets() =>
    [
        .. context.Tickets
            .Include(t => t.Passenger)
            .Include(t => t.Train)
                .ThenInclude(tr => tr.Destination)
            .Include(t => t.CarriageType)
            .OrderBy(t => t.Id)
    ];

    public List<Passenger> GetAllPassengers() =>
    [
        .. context.Passengers
            .Include(p => p.Tickets)
            .OrderBy(p => p.Id)
    ];

    public List<Train> GetAllTrains() =>
        [.. context.Trains.Include(tr => tr.Destination).OrderBy(tr => tr.Id)];

    public List<Destination> GetAllDestinations() =>
        [.. context.Destinations.OrderBy(d => d.Id)];
}
