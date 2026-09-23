using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Destinations;

public class DetailsModel(IDestinationService destinationService) : PageModel
{
    public Destination? Destination { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Destination = await destinationService.GetByIdAsync(id.Value);
        if (Destination == null)
        {
            return NotFound();
        }

        return Page();
    }
}
