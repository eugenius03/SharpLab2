using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Pages.Tickets;

public class DeleteModel(ITicketService ticketService) : PageModel
{
    [BindProperty]
    public Ticket Ticket { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticket = await ticketService.GetByIdAsync(id.Value);
        if (ticket == null)
        {
            return NotFound();
        }

        Ticket = ticket;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        await ticketService.DeleteAsync(id.Value);
        return RedirectToPage("./Index");
    }
}
