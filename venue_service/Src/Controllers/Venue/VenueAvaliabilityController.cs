using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using venue_service.Src.Dtos.Venue;
using venue_service.Src.Interfaces.Venue;
using venue_service.Src.Iterfaces.Venue;

namespace venue_service.Src.Controllers.Venue
{
    [ApiController]
    [Route("api/venue-availability")]
    public class VenueAvaliabilityController : ControllerBase
    {
        private readonly IVenueAvailabilityTime _venueAvailabilityTimeService;

        public VenueAvaliabilityController(IVenueAvailabilityTime venueAvaliabilityTime)
        {
            _venueAvailabilityTimeService = venueAvaliabilityTime;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAvaliableTime([FromBody] CreateVenueAvaliabilityDto dto)
        {
            var avaliableTime = await _venueAvailabilityTimeService.CreateVenueAvailabilityTimeAsync(dto);
            return Ok(avaliableTime);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAvaliableTime([FromQuery] int id)
        {
            var timesToDelete = _venueAvailabilityTimeService.DeleteVenueAvailabilityTimeAsync(id);
            return Ok(timesToDelete);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvaliableTimesByVenue([FromQuery] int venueId)
        {
            var avaliableTimes = await _venueAvailabilityTimeService.ListAvaliableTimesByVenue(venueId);
            return Ok(avaliableTimes);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAvaliabilityTime([FromQuery] int id, [FromBody] UpdateVenueAvaliabilityDto newTimeDto)
        {
            var updatedTime = await _venueAvailabilityTimeService.UpdateAvaliabilityTime(id, newTimeDto);
            return Ok(updatedTime);
        }
    }
}
