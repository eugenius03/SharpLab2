using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Passengers;

public class DeleteModel(IPassengerService passengerService) : PageModel
{
    [BindProperty]
    public Passenger Passenger { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var passenger = await passengerService.GetByIdAsync(id.Value);
        if (passenger == null)
        {
            return NotFound();
        }

        Passenger = passenger;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        await passengerService.DeleteAsync(id.Value);
        return RedirectToPage("./Index");
    }
}
