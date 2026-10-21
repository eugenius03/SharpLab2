using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Hubs;

public class TicketsHub(ITicketService service) : CrudHub<Ticket>(service)
{
}
