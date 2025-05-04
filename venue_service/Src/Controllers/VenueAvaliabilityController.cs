using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos;
using venue_service.Src.Services;

namespace venue_service.Src.Controllers
{
    [ApiController]
    [Route("api/venue-availability")]
    public class VenueAvaliabilityController : ControllerBase
    {
        private readonly IVenueAvaliabilityTime _venueAvaliabilityTimeService;

        public VenueAvaliabilityController(IVenueAvaliabilityTime venueAvaliabilityTime)
        {
            _venueAvaliabilityTimeService = venueAvaliabilityTime;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvaliableTimesByVenue([FromQuery] int venueId)
        {
            var avaliableTimes = await _venueAvaliabilityTimeService.ListAvaliableTimesByVenue(venueId);
            return Ok(avaliableTimes);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAvaliabilityTime([FromQuery] int id, [FromBody] UpdateVenueAvaliabilityDto newTimeDto)
        {
            var updatedTime = await _venueAvaliabilityTimeService.UpdateAvaliabilityTime(id, newTimeDto);
            return Ok(updatedTime);
        }
    }
}
