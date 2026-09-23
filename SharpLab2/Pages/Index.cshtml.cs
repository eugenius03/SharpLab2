using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Services;

namespace SharpLab2.Pages;

public class IndexModel(
    ITicketService ticketService,
    IPassengerService passengerService,
    ITrainService trainService,
    IDestinationService destinationService,
    ICarriageTypeService carriageTypeService) : PageModel
{
    public int TicketsCount { get; private set; }
    public int PassengersCount { get; private set; }
    public int TrainsCount { get; private set; }
    public int DestinationsCount { get; private set; }
    public int CarriageTypesCount { get; private set; }

    public async Task OnGetAsync()
    {
        TicketsCount = (await ticketService.GetAllAsync()).Count;
        PassengersCount = (await passengerService.GetAllAsync()).Count;
        TrainsCount = (await trainService.GetAllAsync()).Count;
        DestinationsCount = (await destinationService.GetAllAsync()).Count;
        CarriageTypesCount = (await carriageTypeService.GetAllAsync()).Count;
    }
}
