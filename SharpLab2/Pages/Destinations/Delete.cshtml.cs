using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Destinations;

public class DeleteModel(IDestinationService destinationService) : PageModel
{
    [BindProperty]
    public Destination Destination { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? id)
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

        Destination = destination;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        await destinationService.DeleteAsync(id.Value);
        return RedirectToPage("./Index");
    }
}
