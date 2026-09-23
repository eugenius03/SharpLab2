using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Controllers;

public class TicketsController(
    ITicketService ticketService,
    IPassengerService passengerService,
    ITrainService trainService,
    ICarriageTypeService carriageTypeService) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await ticketService.GetAllAsync());
    }

    public async Task<IActionResult> Details(int? id)
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

        return View(ticket);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PassengerId,TrainId,CarriageNumber,CarriageTypeId,DepartureDate,UrgencySurcharge")] Ticket ticket)
    {
        if (ModelState.IsValid)
        {
            await ticketService.CreateAsync(ticket);
            return RedirectToAction(nameof(Index));
        }

        await PopulateDropdownsAsync(ticket.PassengerId, ticket.TrainId, ticket.CarriageTypeId);
        return View(ticket);
    }

    public async Task<IActionResult> Edit(int? id)
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

        await PopulateDropdownsAsync(ticket.PassengerId, ticket.TrainId, ticket.CarriageTypeId);
        return View(ticket);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,PassengerId,TrainId,CarriageNumber,CarriageTypeId,DepartureDate,UrgencySurcharge")] Ticket ticket)
    {
        if (id != ticket.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await ticketService.ExistsAsync(id))
            {
                return NotFound();
            }

            await ticketService.UpdateAsync(ticket);
            return RedirectToAction(nameof(Index));
        }

        await PopulateDropdownsAsync(ticket.PassengerId, ticket.TrainId, ticket.CarriageTypeId);
        return View(ticket);
    }

    public async Task<IActionResult> Delete(int? id)
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

        return View(ticket);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await ticketService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdownsAsync(int? selectedPassenger = null, int? selectedTrain = null, int? selectedCarriageType = null)
    {
        var passengers = await passengerService.GetAllAsync();
        ViewBag.PassengerId = new SelectList(passengers, "Id", "FullName", selectedPassenger);

        var trains = await trainService.GetAllAsync();
        var trainList = trains.Select(t => new
        {
            t.Id,
            Display = t.TrainNumber + " (" + t.TrainType + " -> " + (t.Destination != null ? t.Destination.Name : "") + ")"
        }).ToList();
        ViewBag.TrainId = new SelectList(trainList, "Id", "Display", selectedTrain);

        var carriageTypes = await carriageTypeService.GetAllAsync();
        ViewBag.CarriageTypeId = new SelectList(carriageTypes, "Id", "TypeName", selectedCarriageType);
    }
}
