using Microsoft.AspNetCore.Mvc;
using SharpLab2.DTOs;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PassengersController(IPassengerService passengerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PassengerResponse>>> GetAll()
    {
        var passengers = await passengerService.GetAllAsync();
        var responses = passengers.Select(MapToResponse).ToList();
        return Ok(responses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PassengerResponse>> GetById(int id)
    {
        var passenger = await passengerService.GetByIdAsync(id);
        if (passenger == null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(passenger));
    }

    [HttpPost]
    public async Task<ActionResult<PassengerResponse>> Create([FromBody] CreatePassengerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var passenger = new Passenger
        {
            FullName = request.FullName,
            Address = request.Address,
            Phone = request.Phone
        };

        await passengerService.CreateAsync(passenger);
        return CreatedAtAction(nameof(GetById), new { id = passenger.Id }, MapToResponse(passenger));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreatePassengerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var passenger = await passengerService.GetByIdAsync(id);
        if (passenger == null)
        {
            return NotFound();
        }

        passenger.FullName = request.FullName;
        passenger.Address = request.Address;
        passenger.Phone = request.Phone;

        await passengerService.UpdateAsync(passenger);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await passengerService.ExistsAsync(id))
        {
            return NotFound();
        }

        await passengerService.DeleteAsync(id);
        return NoContent();
    }

    private static PassengerResponse MapToResponse(Passenger p) => new(
        p.Id,
        p.FullName,
        p.Address,
        p.Phone,
        p.Tickets.Count
    );
}
