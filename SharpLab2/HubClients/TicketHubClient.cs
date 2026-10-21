using Microsoft.AspNetCore.Components;
using SharpLab2.Models;

namespace SharpLab2.HubClients;

public class TicketHubClient(NavigationManager navigation)
    : HubClient<Ticket>(navigation, "/hubs/tickets"), ITicketHubClient
{
}
