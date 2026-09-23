using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Passengers;

public class CreateModel(IPassengerService passengerService) : PageModel
{
    [BindProperty]
    public Passenger Passenger { get; set; } = new();

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

        await passengerService.CreateAsync(Passenger);
        return RedirectToPage("./Index");
    }
}
