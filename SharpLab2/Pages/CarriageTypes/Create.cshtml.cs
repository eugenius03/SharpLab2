using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.CarriageTypes;

public class CreateModel(ICarriageTypeService carriageTypeService) : PageModel
{
    [BindProperty]
    public CarriageType CarriageType { get; set; } = new();

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await carriageTypeService.CreateAsync(CarriageType);
        return RedirectToPage("./Index");
    }
}
