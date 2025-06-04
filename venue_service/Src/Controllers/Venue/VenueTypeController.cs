using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Interfaces.VenueInterfaces;

namespace venue_service.Src.Controllers.Venue
{
    [ApiController]
    [Route("api/venue-types")]
    public class VenueTypeController : ControllerBase
    {
        private readonly IVenueType _venueTypeService;

        public VenueTypeController(IVenueType venueTypeService)
        {
            _venueTypeService = venueTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVenueTypes()
        {
            var venueTypes = await _venueTypeService.GetAllVenueTypes();
            return Ok(venueTypes);
        }


        
    }
}
