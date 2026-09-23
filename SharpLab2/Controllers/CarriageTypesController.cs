using Microsoft.AspNetCore.Mvc;
using SharpLab2.DTOs;
using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarriageTypesController(ICarriageTypeService carriageTypeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CarriageTypeResponse>>> GetAll()
    {
        var carriageTypes = await carriageTypeService.GetAllAsync();
        var responses = carriageTypes.Select(MapToResponse).ToList();
        return Ok(responses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CarriageTypeResponse>> GetById(int id)
    {
        var carriageType = await carriageTypeService.GetByIdAsync(id);
        if (carriageType == null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(carriageType));
    }

    [HttpPost]
    public async Task<ActionResult<CarriageTypeResponse>> Create([FromBody] CreateCarriageTypeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var carriageType = new CarriageType
        {
            TypeName = request.TypeName,
            Surcharge = request.Surcharge
        };

        await carriageTypeService.CreateAsync(carriageType);
        return CreatedAtAction(nameof(GetById), new { id = carriageType.Id }, MapToResponse(carriageType));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateCarriageTypeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var carriageType = await carriageTypeService.GetByIdAsync(id);
        if (carriageType == null)
        {
            return NotFound();
        }

        carriageType.TypeName = request.TypeName;
        carriageType.Surcharge = request.Surcharge;

        await carriageTypeService.UpdateAsync(carriageType);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await carriageTypeService.ExistsAsync(id))
        {
            return NotFound();
        }

        await carriageTypeService.DeleteAsync(id);
        return NoContent();
    }

    private static CarriageTypeResponse MapToResponse(CarriageType c) => new(
        c.Id,
        c.TypeName,
        c.Surcharge,
        c.Tickets.Count
    );
}
