using Microsoft.AspNetCore.Components;
using SharpLab2.Models;

namespace SharpLab2.HubClients;

public class CarriageTypeHubClient(NavigationManager navigation)
    : HubClient<CarriageType>(navigation, "/hubs/carriagetypes"), ICarriageTypeHubClient
{
}
