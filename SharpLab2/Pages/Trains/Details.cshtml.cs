using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Trains;

public class DetailsModel(ITrainService trainService) : PageModel
{
    public Train? Train { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Train = await trainService.GetByIdAsync(id.Value);
        if (Train == null)
        {
            return NotFound();
        }

        return Page();
    }
}
