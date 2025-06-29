﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.Venue;
using venue_service.Src.Interfaces.VenueInterfaces;

namespace venue_service.Src.Controllers.Venue
{
    [Authorize]
    [ApiController]
    [Route("api/venues")]
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
        public async Task<ActionResult<VenuesResponseDto>> GetVenues(
            [FromQuery] int? venueTypeId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int? minCapacity,
            [FromQuery] int? maxCapacity,
            [FromQuery] string? name,
            [FromQuery] List<int>? sportId,
            [FromQuery] bool? isReserved)
        {
            var result = await _venueService.GetVenuesAsync(venueTypeId, from, to, minCapacity, maxCapacity, name, sportId, isReserved);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVenue([FromRoute] int id, [FromBody] UpdateVenueRequestDto dto)
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

        [HttpGet("by-owner/{id}")]
        public async Task<IActionResult> GetVenueByOwnerId([FromRoute] int id)
        {
            var result = await _venueService.ListVenuesByOwner(id);
            return Ok(result);
        }

        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> GetResultAsync([FromRoute] int id)
        {
            var result = await _venueService.GetVenueByIdAsync(id);
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
