using Microsoft.AspNetCore.Mvc;
using SharpLab2.Services;

namespace SharpLab2.Controllers;

public class HomeController(
    ITicketService ticketService,
    IPassengerService passengerService,
    ITrainService trainService,
    IDestinationService destinationService,
    ICarriageTypeService carriageTypeService) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.TicketsCount = (await ticketService.GetAllAsync()).Count;
        ViewBag.PassengersCount = (await passengerService.GetAllAsync()).Count;
        ViewBag.TrainsCount = (await trainService.GetAllAsync()).Count;
        ViewBag.DestinationsCount = (await destinationService.GetAllAsync()).Count;
        ViewBag.CarriageTypesCount = (await carriageTypeService.GetAllAsync()).Count;

        return View();
    }
}
