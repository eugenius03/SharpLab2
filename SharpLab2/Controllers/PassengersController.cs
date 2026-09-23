using Microsoft.AspNetCore.Mvc;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Controllers;

public class PassengersController(IPassengerService passengerService) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await passengerService.GetAllAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var passenger = await passengerService.GetByIdAsync(id.Value);
        if (passenger == null)
        {
            return NotFound();
        }

        return View(passenger);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FullName,Address,Phone")] Passenger passenger)
    {
        if (ModelState.IsValid)
        {
            await passengerService.CreateAsync(passenger);
            return RedirectToAction(nameof(Index));
        }

        return View(passenger);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var passenger = await passengerService.GetByIdAsync(id.Value);
        if (passenger == null)
        {
            return NotFound();
        }

        return View(passenger);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Address,Phone")] Passenger passenger)
    {
        if (id != passenger.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await passengerService.ExistsAsync(id))
            {
                return NotFound();
            }

            await passengerService.UpdateAsync(passenger);
            return RedirectToAction(nameof(Index));
        }

        return View(passenger);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var passenger = await passengerService.GetByIdAsync(id.Value);
        if (passenger == null)
        {
            return NotFound();
        }

        return View(passenger);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await passengerService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
