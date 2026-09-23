using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Trains;

public class DeleteModel(ITrainService trainService) : PageModel
{
    [BindProperty]
    public Train Train { get; set; } = null!;

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
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        await trainService.DeleteAsync(id.Value);
        return RedirectToPage("./Index");
    }
}
