using Microsoft.AspNetCore.Mvc;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Controllers;

public class CarriageTypesController(ICarriageTypeService carriageTypeService) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await carriageTypeService.GetAllAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carriageType = await carriageTypeService.GetByIdAsync(id.Value);
        if (carriageType == null)
        {
            return NotFound();
        }

        return View(carriageType);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TypeName,Surcharge")] CarriageType carriageType)
    {
        if (ModelState.IsValid)
        {
            await carriageTypeService.CreateAsync(carriageType);
            return RedirectToAction(nameof(Index));
        }

        return View(carriageType);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carriageType = await carriageTypeService.GetByIdAsync(id.Value);
        if (carriageType == null)
        {
            return NotFound();
        }

        return View(carriageType);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,TypeName,Surcharge")] CarriageType carriageType)
    {
        if (id != carriageType.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (!await carriageTypeService.ExistsAsync(id))
            {
                return NotFound();
            }

            await carriageTypeService.UpdateAsync(carriageType);
            return RedirectToAction(nameof(Index));
        }

        return View(carriageType);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carriageType = await carriageTypeService.GetByIdAsync(id.Value);
        if (carriageType == null)
        {
            return NotFound();
        }

        return View(carriageType);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await carriageTypeService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
