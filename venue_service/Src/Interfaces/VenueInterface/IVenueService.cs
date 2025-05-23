using venue_service.Src.Dtos.Venue;
using venue_service.Src.Models;

namespace venue_service.Src.Interfaces.Venue;

public interface IVenueService
{
    Task<VenueResponseDto> CreateVenueAsync(CreateVenueRequestDto dto);
    Task<VenuesResponseDto> ListVenuesAsync();
    Task<VenuesResponseDto> DeleteVenuesAsync(int[] ids);
    Task<VenueResponseDto> UpdateVenueAsync(int id, UpdateVenueRequestDto dto);
    Task<VenuesResponseDto> ListVenuesByOwner(int id);
    Task<UpdateVenueImageResponseDto> AddVenueImagesAsync(UpdateVenueImageDto dto);
    Task DeleteVenueImageAsync(int venueId, string imageUrl);

}
