using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Passengers;

public class DetailsModel(IPassengerService passengerService) : PageModel
{
    public Passenger? Passenger { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Passenger = await passengerService.GetByIdAsync(id.Value);
        if (Passenger == null)
        {
            return NotFound();
        }

        return Page();
    }
}
