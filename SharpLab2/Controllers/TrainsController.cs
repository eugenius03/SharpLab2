using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Controllers;

public class TrainsController(ITrainService trainService, IDestinationService destinationService) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await trainService.GetAllAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var train = await trainService.GetByIdAsync(id.Value);
        if (train == null)
        {
            return NotFound();
        }

        return View(train);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDropdownAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TrainNumber,TrainType,DestinationId,DepartureTime,ArrivalTime")] Train train)
    {
        if (ModelState.IsValid)
        {
            await trainService.CreateAsync(train);
            return RedirectToAction(nameof(Index));
        }

        await PopulateDropdownAsync(train.DestinationId);
        return View(train);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var train = await trainService.GetByIdAsync(id.Value);
        if (train == null)
        {
            return NotFound();
        }

        await PopulateDropdownAsync(train.DestinationId);
        return View(train);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,TrainNumber,TrainType,DestinationId,DepartureTime,ArrivalTime")] Train train)
    {
        if (id != train.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await trainService.ExistsAsync(id))
            {
                return NotFound();
            }

            await trainService.UpdateAsync(train);
            return RedirectToAction(nameof(Index));
        }

        await PopulateDropdownAsync(train.DestinationId);
        return View(train);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var train = await trainService.GetByIdAsync(id.Value);
        if (train == null)
        {
            return NotFound();
        }

        return View(train);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await trainService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdownAsync(int? selectedDestination = null)
    {
        var destinations = await destinationService.GetAllAsync();
        ViewBag.DestinationId = new SelectList(destinations, "Id", "Name", selectedDestination);
    }
}
