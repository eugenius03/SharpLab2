using Microsoft.AspNetCore.Components;
using SharpLab2.Models;

namespace SharpLab2.HubClients;

public class DestinationHubClient(NavigationManager navigation)
    : HubClient<Destination>(navigation, "/hubs/destinations"), IDestinationHubClient
{
}
