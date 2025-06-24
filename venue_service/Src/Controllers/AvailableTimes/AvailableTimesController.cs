using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.AvailabilityTimes;
using venue_service.Src.Interfaces.AvailableTimesInterfaces;

namespace venue_service.Src.Controllers.AvailableTimes;

[ApiController]
[Authorize]
[Route("api/available-times")]
public class AvailableTimesController : ControllerBase
{
    private readonly IAvailableTimesService _availableTimesService;

    public AvailableTimesController(IAvailableTimesService availableTimesService)
    {
        _availableTimesService = availableTimesService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateVenueAvailabilityTime([FromBody] CreateVenueAvailabilityTimeDto requestDto)
    {
        var result = await _availableTimesService.CreateVenueAvailabilityTime(requestDto);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetVenueAvailabilityTimes()
    {
        var result = await _availableTimesService.GetVenueAvailabilityTimes();
        return Ok(result);
    }

    [HttpGet("by-id/{id}")]
    public async Task<IActionResult> GetVenueAvailabilityTimeById([FromRoute] int id)
    {
        var result = await _availableTimesService.GetVenueAvailabilityTimeById(id);
        return Ok(result);
    }

    [HttpPut("{availabilityTimeId}")]
    public async Task<IActionResult> UpdateVenueAvailabilityTime([FromRoute] int availabilityTimeId, [FromBody] UpdateVenueAvailabilityTimeDto requestDto)
    {
        var result = await _availableTimesService.UpdateVenueAvailabilityTime(availabilityTimeId, requestDto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVenueAvailabilityTime([FromRoute] int id)
    {
        var result = await _availableTimesService.DeleteVenueAvailabilityTime(id);
        return Ok(result);
    }


}
