using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.CarriageTypes;

public class EditModel(ICarriageTypeService carriageTypeService) : PageModel
{
    [BindProperty]
    public CarriageType CarriageType { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? id)
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

        CarriageType = carriageType;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (!await carriageTypeService.ExistsAsync(CarriageType.Id))
        {
            return NotFound();
        }

        await carriageTypeService.UpdateAsync(CarriageType);
        return RedirectToPage("./Index");
    }
}
