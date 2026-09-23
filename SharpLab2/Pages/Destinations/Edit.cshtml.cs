using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Destinations;

public class EditModel(IDestinationService destinationService) : PageModel
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (!await destinationService.ExistsAsync(Destination.Id))
        {
            return NotFound();
        }

        await destinationService.UpdateAsync(Destination);
        return RedirectToPage("./Index");
    }
}
