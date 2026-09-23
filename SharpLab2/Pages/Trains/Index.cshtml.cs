using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Trains;

public class IndexModel(ITrainService trainService) : PageModel
{
    public IList<Train> Trains { get; set; } = [];

    public async Task OnGetAsync()
    {
        Trains = await trainService.GetAllAsync();
    }
}
