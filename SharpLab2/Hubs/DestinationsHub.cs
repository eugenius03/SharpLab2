using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Hubs;

public class DestinationsHub(IDestinationService service) : CrudHub<Destination>(service)
{
}
