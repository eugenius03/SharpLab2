using Microsoft.AspNetCore.Mvc;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Controllers;

public class DestinationsController(IDestinationService destinationService) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await destinationService.GetAllAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var destination = await destinationService.GetByIdAsync(id.Value);
        if (destination == null)
        {
            return NotFound();
        }

        return View(destination);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,DistanceKm,BaseFare")] Destination destination)
    {
        if (ModelState.IsValid)
        {
            await destinationService.CreateAsync(destination);
            return RedirectToAction(nameof(Index));
        }

        return View(destination);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var destination = await destinationService.GetByIdAsync(id.Value);
        if (destination == null)
        {
            return NotFound();
        }

        return View(destination);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DistanceKm,BaseFare")] Destination destination)
    {
        if (id != destination.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await destinationService.ExistsAsync(id))
            {
                return NotFound();
            }

            await destinationService.UpdateAsync(destination);
            return RedirectToAction(nameof(Index));
        }

        return View(destination);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var destination = await destinationService.GetByIdAsync(id.Value);
        if (destination == null)
        {
            return NotFound();
        }

        return View(destination);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await destinationService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
