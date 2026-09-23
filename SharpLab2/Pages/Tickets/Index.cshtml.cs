using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Tickets;

public class IndexModel(ITicketService ticketService) : PageModel
{
    public IList<Ticket> Tickets { get; set; } = [];

    public async Task OnGetAsync()
    {
        Tickets = await ticketService.GetAllAsync();
    }
}
