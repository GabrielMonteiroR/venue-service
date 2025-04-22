using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        [Route("api/venue-availability")]
        public async Task<IActionResult> GetAvaliableTimesByVenue([FromBody] int venueId)
        {
            var avaliableTimes = await _venueAvaliabilityTimeService.ListAvaliableTimesByVenue(venueId);
            return Ok(avaliableTimes);
        }
    }
}
