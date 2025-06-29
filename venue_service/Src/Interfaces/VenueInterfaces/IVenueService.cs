﻿using venue_service.Src.Dtos.Venue;
using venue_service.Src.Models;

namespace venue_service.Src.Interfaces.VenueInterfaces;

public interface IVenueService
{
    Task<VenueResponseDto> CreateVenueAsync(CreateVenueRequestDto dto);
    Task<VenuesResponseDto> DeleteVenuesAsync(int[] ids);
    Task<VenueResponseDto> UpdateVenueAsync(int id, UpdateVenueRequestDto dto);
    Task<VenuesResponseDto> GetVenuesAsync(int? venueTypeId = null, DateTime? from = null, DateTime? to = null, int? minCapacity = null, int? maxCapacity = null, string? name = null, List<int>? sportId = null, bool? isReserved = null);
    Task<VenuesResponseDto> ListVenuesByOwner(int id);
    Task<UpdateVenueImageResponseDto> AddVenueImagesAsync(UpdateVenueImageDto dto);
    Task DeleteVenueImageAsync(int venueId, string imageUrl);
    Task<VenueResponseDto> GetVenueByIdAsync(int id);

}
