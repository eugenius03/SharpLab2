using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.CarriageTypes;

public class IndexModel(ICarriageTypeService carriageTypeService) : PageModel
{
    public IList<CarriageType> CarriageTypes { get; set; } = [];

    public async Task OnGetAsync()
    {
        CarriageTypes = await carriageTypeService.GetAllAsync();
    }
}
