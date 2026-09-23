using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Tickets;

public class DetailsModel(ITicketService ticketService) : PageModel
{
    public Ticket? Ticket { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Ticket = await ticketService.GetByIdAsync(id.Value);
        if (Ticket == null)
        {
            return NotFound();
        }

        return Page();
    }
}
