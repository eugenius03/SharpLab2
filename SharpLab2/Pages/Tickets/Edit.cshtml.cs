using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Tickets;

public class EditModel(
    ITicketService ticketService,
    IPassengerService passengerService,
    ITrainService trainService,
    ICarriageTypeService carriageTypeService) : PageModel
{
    [BindProperty]
    public Ticket Ticket { get; set; } = null!;

    public SelectList PassengerList { get; set; } = null!;
    public SelectList TrainList { get; set; } = null!;
    public SelectList CarriageTypeList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticket = await ticketService.GetByIdAsync(id.Value);
        if (ticket == null)
        {
            return NotFound();
        }

        Ticket = ticket;
        await PopulateDropdownsAsync(Ticket.PassengerId, Ticket.TrainId, Ticket.CarriageTypeId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(Ticket.PassengerId, Ticket.TrainId, Ticket.CarriageTypeId);
            return Page();
        }

        if (!await ticketService.ExistsAsync(Ticket.Id))
        {
            return NotFound();
        }

        await ticketService.UpdateAsync(Ticket);
        return RedirectToPage("./Index");
    }

    private async Task PopulateDropdownsAsync(int? selectedPassenger = null, int? selectedTrain = null, int? selectedCarriageType = null)
    {
        var passengers = await passengerService.GetAllAsync();
        PassengerList = new SelectList(passengers, "Id", "FullName", selectedPassenger);

        var trains = await trainService.GetAllAsync();
        var trainDisplayList = trains.Select(t => new
        {
            t.Id,
            Display = t.TrainNumber + " (" + t.TrainType + " -> " + (t.Destination != null ? t.Destination.Name : "") + ")"
        }).ToList();
        TrainList = new SelectList(trainDisplayList, "Id", "Display", selectedTrain);

        var carriageTypes = await carriageTypeService.GetAllAsync();
        CarriageTypeList = new SelectList(carriageTypes, "Id", "TypeName", selectedCarriageType);
    }
}
