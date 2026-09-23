using Microsoft.AspNetCore.Mvc;
using SharpLab2.DTOs;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController(ITicketService ticketService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TicketResponse>>> GetAll()
    {
        var tickets = await ticketService.GetAllAsync();
        var responses = tickets.Select(MapToResponse).ToList();
        return Ok(responses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TicketResponse>> GetById(int id)
    {
        var ticket = await ticketService.GetByIdAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(ticket));
    }

    [HttpPost]
    public async Task<ActionResult<TicketResponse>> Create([FromBody] CreateTicketRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var ticket = new Ticket
        {
            PassengerId = request.PassengerId,
            TrainId = request.TrainId,
            CarriageNumber = request.CarriageNumber,
            CarriageTypeId = request.CarriageTypeId,
            DepartureDate = request.DepartureDate,
            UrgencySurcharge = request.UrgencySurcharge
        };

        await ticketService.CreateAsync(ticket);
        var created = await ticketService.GetByIdAsync(ticket.Id);

        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, MapToResponse(created ?? ticket));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateTicketRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var ticket = await ticketService.GetByIdAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        ticket.PassengerId = request.PassengerId;
        ticket.TrainId = request.TrainId;
        ticket.CarriageNumber = request.CarriageNumber;
        ticket.CarriageTypeId = request.CarriageTypeId;
        ticket.DepartureDate = request.DepartureDate;
        ticket.UrgencySurcharge = request.UrgencySurcharge;

        await ticketService.UpdateAsync(ticket);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await ticketService.ExistsAsync(id))
        {
            return NotFound();
        }

        await ticketService.DeleteAsync(id);
        return NoContent();
    }

    private static TicketResponse MapToResponse(Ticket t) => new(
        t.Id,
        t.PassengerId,
        t.Passenger?.FullName,
        t.Passenger?.Address,
        t.Passenger?.Phone,
        t.TrainId,
        t.Train?.TrainNumber,
        t.Train?.TrainType,
        t.Train?.DepartureTime,
        t.Train?.ArrivalTime,
        t.Train?.Destination?.Name,
        t.Train?.Destination?.DistanceKm ?? 0,
        t.Train?.Destination?.BaseFare ?? 0,
        t.CarriageNumber,
        t.CarriageTypeId,
        t.CarriageType?.TypeName,
        t.CarriageType?.Surcharge ?? 0,
        t.DepartureDate,
        t.UrgencySurcharge,
        t.GetTotalPrice()
    );
}
