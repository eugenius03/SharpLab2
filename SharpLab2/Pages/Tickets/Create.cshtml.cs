using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Tickets;

public class CreateModel(
    ITicketService ticketService,
    IPassengerService passengerService,
    ITrainService trainService,
    ICarriageTypeService carriageTypeService) : PageModel
{
    [BindProperty]
    public Ticket Ticket { get; set; } = new();

    public SelectList PassengerList { get; set; } = null!;
    public SelectList TrainList { get; set; } = null!;
    public SelectList CarriageTypeList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        await PopulateDropdownsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync();
            return Page();
        }

        await ticketService.CreateAsync(Ticket);
        return RedirectToPage("./Index");
    }

    private async Task PopulateDropdownsAsync()
    {
        var passengers = await passengerService.GetAllAsync();
        PassengerList = new SelectList(passengers, "Id", "FullName");

        var trains = await trainService.GetAllAsync();
        var trainDisplayList = trains.Select(t => new
        {
            t.Id,
            Display = t.TrainNumber + " (" + t.TrainType + " -> " + (t.Destination != null ? t.Destination.Name : "") + ")"
        }).ToList();
        TrainList = new SelectList(trainDisplayList, "Id", "Display");

        var carriageTypes = await carriageTypeService.GetAllAsync();
        CarriageTypeList = new SelectList(carriageTypes, "Id", "TypeName");
    }
}
