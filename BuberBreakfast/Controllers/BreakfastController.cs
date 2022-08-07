using BuberBreakfast.Contracts;
using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using BuberBreakfast.Services.Breakfasts;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

public class BreakfastsController : ApiController
{
    private readonly IBreakfastService _breakfastService;

    public BreakfastsController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }

  [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        
        var requestToBreakfastResult = Breakfast.From(request);
        if (requestToBreakfastResult.IsError)
        {
            return BadRequest(requestToBreakfastResult.Errors);
        }
        var breakfast = requestToBreakfastResult.Value;
        var breakfastResult = _breakfastService.CreateBreakfast(breakfast);
        if (breakfastResult.IsError)
        {
            return Problem(breakfastResult.Errors);
        }
        return breakfastResult.Match(
            created => CreatedAtGetBreakfast(breakfast),
            errors => Problem(errors)
        );
    }
    [HttpGet]
    public IActionResult GetAllBreakfast()
    {
        return Ok(_breakfastService.GetAll());
    }
    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        var breakfastResult = _breakfastService.GetBreakfast(id);
        return breakfastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)),
            errors => Problem(errors)
        );
    }
    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        var requestToBreakfastResult = Breakfast.From(id, request);
        if (requestToBreakfastResult.IsError)
        {
            return Problem(requestToBreakfastResult.Errors);
        }
        var breakfast = requestToBreakfastResult.Value;
        var breakfastResult = _breakfastService.UpsertBreakfast(breakfast);
        return breakfastResult.Match(
            upserted => upserted.IsNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(),
            errors => Problem(errors)
        );
    }
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        var breakfastResult = _breakfastService.DeleteBreakfast(id);
        return breakfastResult.Match(
            deleted => NoContent(),
            errors => Problem(errors)
        );
    }
    private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
            breakfast.Id,
            breakfast.Name,
            breakfast.Description,
            breakfast.StartDateTime,
            breakfast.EndDateTime,
            breakfast.LastModifiedDateTime,
            breakfast.Savory,
            breakfast.Sweet
        );
    }
    private CreatedAtActionResult CreatedAtGetBreakfast(Breakfast breakfast)
    {
        return CreatedAtAction(
            actionName: nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },
            value: MapBreakfastResponse(breakfast)
        );
    }
}
