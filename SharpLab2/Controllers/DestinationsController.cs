using Microsoft.AspNetCore.Mvc;
using SharpLab2.DTOs;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DestinationsController(IDestinationService destinationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<DestinationResponse>>> GetAll()
    {
        var destinations = await destinationService.GetAllAsync();
        var responses = destinations.Select(MapToResponse).ToList();
        return Ok(responses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DestinationResponse>> GetById(int id)
    {
        var destination = await destinationService.GetByIdAsync(id);
        if (destination == null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(destination));
    }

    [HttpPost]
    public async Task<ActionResult<DestinationResponse>> Create([FromBody] CreateDestinationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var destination = new Destination
        {
            Name = request.Name,
            DistanceKm = request.DistanceKm,
            BaseFare = request.BaseFare
        };

        await destinationService.CreateAsync(destination);
        return CreatedAtAction(nameof(GetById), new { id = destination.Id }, MapToResponse(destination));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateDestinationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var destination = await destinationService.GetByIdAsync(id);
        if (destination == null)
        {
            return NotFound();
        }

        destination.Name = request.Name;
        destination.DistanceKm = request.DistanceKm;
        destination.BaseFare = request.BaseFare;

        await destinationService.UpdateAsync(destination);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await destinationService.ExistsAsync(id))
        {
            return NotFound();
        }

        await destinationService.DeleteAsync(id);
        return NoContent();
    }

    private static DestinationResponse MapToResponse(Destination d) => new(
        d.Id,
        d.Name,
        d.DistanceKm,
        d.BaseFare,
        d.Trains.Count
    );
}
