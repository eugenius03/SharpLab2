using Microsoft.AspNetCore.Mvc;
using SharpLab2.DTOs;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainsController(ITrainService trainService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TrainResponse>>> GetAll()
    {
        var trains = await trainService.GetAllAsync();
        var responses = trains.Select(MapToResponse).ToList();
        return Ok(responses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TrainResponse>> GetById(int id)
    {
        var train = await trainService.GetByIdAsync(id);
        if (train == null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(train));
    }

    [HttpPost]
    public async Task<ActionResult<TrainResponse>> Create([FromBody] CreateTrainRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var train = new Train
        {
            TrainNumber = request.TrainNumber,
            TrainType = request.TrainType,
            DestinationId = request.DestinationId,
            DepartureTime = request.DepartureTime,
            ArrivalTime = request.ArrivalTime
        };

        await trainService.CreateAsync(train);
        var created = await trainService.GetByIdAsync(train.Id);

        return CreatedAtAction(nameof(GetById), new { id = train.Id }, MapToResponse(created ?? train));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateTrainRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var train = await trainService.GetByIdAsync(id);
        if (train == null)
        {
            return NotFound();
        }

        train.TrainNumber = request.TrainNumber;
        train.TrainType = request.TrainType;
        train.DestinationId = request.DestinationId;
        train.DepartureTime = request.DepartureTime;
        train.ArrivalTime = request.ArrivalTime;

        await trainService.UpdateAsync(train);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await trainService.ExistsAsync(id))
        {
            return NotFound();
        }

        await trainService.DeleteAsync(id);
        return NoContent();
    }

    private static TrainResponse MapToResponse(Train t) => new(
        t.Id,
        t.TrainNumber,
        t.TrainType,
        t.DestinationId,
        t.Destination?.Name,
        t.DepartureTime,
        t.ArrivalTime,
        t.Tickets.Count
    );
}
