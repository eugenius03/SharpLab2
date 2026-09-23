using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Trains;

public class CreateModel(ITrainService trainService, IDestinationService destinationService) : PageModel
{
    [BindProperty]
    public Train Train { get; set; } = new();

    public SelectList DestinationList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        await PopulateDropdownAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownAsync();
            return Page();
        }

        await trainService.CreateAsync(Train);
        return RedirectToPage("./Index");
    }

    private async Task PopulateDropdownAsync()
    {
        var destinations = await destinationService.GetAllAsync();
        DestinationList = new SelectList(destinations, "Id", "Name");
    }
}
