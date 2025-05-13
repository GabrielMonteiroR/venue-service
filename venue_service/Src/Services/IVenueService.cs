using venue_service.Src.Dtos;
using venue_service.Src.Models;

namespace venue_service.Src.Services;

public interface IVenueService
{
    Task<VenueResponseDto> CreateVenueAsync(CreateVenueRequestDto dto);
    Task<VenuesResponseDto> ListVenuesAsync();
    Task<VenuesResponseDto> DeleteVenuesAsync(int[] ids);
    Task<VenueResponseDto> UpdateVenueAsync(int id, UpdateVenueRequestDto dto);
    Task<VenuesResponseDto> ListVenuesByOwner(int id);
    Task<UpdateVenueImageResponseDto> UpdateVenueImageAsync(UpdateVenueImageDto dto);
}
