using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Destinations;

public class CreateModel(IDestinationService destinationService) : PageModel
{
    [BindProperty]
    public Destination Destination { get; set; } = new();

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

        await destinationService.CreateAsync(Destination);
        return RedirectToPage("./Index");
    }
}
