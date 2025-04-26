using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos;
using venue_service.Src.Services;

namespace venue_service.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenueController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVenue([FromBody] CreateVenueRequestDto dto)
        {
            var result = await _venueService.CreateVenueAsync(dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> ListVenues()
        {
            var result = await _venueService.ListVenuesAsync();
            return Ok(result);
        }
    }
}
