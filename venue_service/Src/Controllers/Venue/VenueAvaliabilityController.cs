using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using venue_service.Src.Dtos.Venue;
using venue_service.Src.Services;

namespace venue_service.Src.Controllers.Venue
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

        [HttpPost]
        public async Task<IActionResult> CreateAvaliableTime([FromBody] CreateVenueAvaliabilityDto dto)
        {
            var avaliableTime = await _venueAvaliabilityTimeService.CreateVenueAvailabilityTimeAsync(dto);
            return Ok(avaliableTime);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAvaliableTime([FromQuery] int id)
        {
            var timesToDelete = _venueAvaliabilityTimeService.DeleteVenueAvailabilityTimeAsync(id);
            return Ok(timesToDelete);
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
