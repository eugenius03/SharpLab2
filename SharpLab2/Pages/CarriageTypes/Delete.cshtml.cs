using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.CarriageTypes;

public class DeleteModel(ICarriageTypeService carriageTypeService) : PageModel
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        await carriageTypeService.DeleteAsync(id.Value);
        return RedirectToPage("./Index");
    }
}
