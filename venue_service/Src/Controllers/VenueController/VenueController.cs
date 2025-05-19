using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.Venue;
using venue_service.Src.Iterfaces.Venue;

namespace venue_service.Src.Controllers.Venue
{
    [ApiController]
    [Route("api/venue")]
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

        [HttpPut]
        public async Task<IActionResult> UpdateVenue([FromQuery]int id, [FromBody] UpdateVenueRequestDto dto)
        {
            var result = await _venueService.UpdateVenueAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVenues([FromQuery] int[] ids)
        {
            var result = await _venueService.DeleteVenuesAsync(ids);
            return Ok(result);
        }

        [HttpGet("by-owner")]
        public async Task<IActionResult> GetVenueByOwnerId([FromQuery] int id)
        {
            var result = await _venueService.ListVenuesByOwner(id);
            return Ok(result);
        }

        [HttpPatch("image/add")]
        public async Task<IActionResult> AddVenueImages([FromBody] UpdateVenueImageDto dto)
        {
            var result = await _venueService.AddVenueImagesAsync(dto);
            return Ok(result);
        }

        [HttpDelete("image")]
        public async Task<IActionResult> DeleteVenueImage([FromQuery] string url, [FromQuery] int venueId)
        {
            await _venueService.DeleteVenueImageAsync(venueId, url);
            return NoContent();
        }


    }
}
