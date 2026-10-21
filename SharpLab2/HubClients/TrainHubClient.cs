using Microsoft.AspNetCore.Components;
using SharpLab2.Models;

namespace SharpLab2.HubClients;

public class TrainHubClient(NavigationManager navigation)
    : HubClient<Train>(navigation, "/hubs/trains"), ITrainHubClient
{
}
