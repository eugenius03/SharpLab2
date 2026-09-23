using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Destinations;

public class IndexModel(IDestinationService destinationService) : PageModel
{
    public IList<Destination> Destinations { get; set; } = [];

    public async Task OnGetAsync()
    {
        Destinations = await destinationService.GetAllAsync();
    }
}
