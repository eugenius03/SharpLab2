using Microsoft.AspNetCore.Components;
using SharpLab2.Models;

namespace SharpLab2.HubClients;

public class PassengerHubClient(NavigationManager navigation)
    : HubClient<Passenger>(navigation, "/hubs/passengers"), IPassengerHubClient
{
}
