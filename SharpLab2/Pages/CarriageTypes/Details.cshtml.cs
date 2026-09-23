using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.CarriageTypes;

public class DetailsModel(ICarriageTypeService carriageTypeService) : PageModel
{
    public CarriageType? CarriageType { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        CarriageType = await carriageTypeService.GetByIdAsync(id.Value);
        if (CarriageType == null)
        {
            return NotFound();
        }

        return Page();
    }
}
