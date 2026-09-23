using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Trains;

public class EditModel(ITrainService trainService, IDestinationService destinationService) : PageModel
{
    [BindProperty]
    public Train Train { get; set; } = null!;

    public SelectList DestinationList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var train = await trainService.GetByIdAsync(id.Value);
        if (train == null)
        {
            return NotFound();
        }

        Train = train;
        await PopulateDropdownAsync(Train.DestinationId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownAsync(Train.DestinationId);
            return Page();
        }

        if (!await trainService.ExistsAsync(Train.Id))
        {
            return NotFound();
        }

        await trainService.UpdateAsync(Train);
        return RedirectToPage("./Index");
    }

    private async Task PopulateDropdownAsync(int? selectedDestination = null)
    {
        var destinations = await destinationService.GetAllAsync();
        DestinationList = new SelectList(destinations, "Id", "Name", selectedDestination);
    }
}
