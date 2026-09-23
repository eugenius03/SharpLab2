using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Passengers;

public class IndexModel(IPassengerService passengerService) : PageModel
{
    public IList<Passenger> Passengers { get; set; } = [];

    public async Task OnGetAsync()
    {
        Passengers = await passengerService.GetAllAsync();
    }
}
