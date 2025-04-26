using venue_service.Src.Dtos;

namespace venue_service.Src.Services;

public interface IVenueService
{
    Task<VenueResponseDto> CreateVenueAsync(CreateVenueRequestDto dto);
    Task<VenuesResponseDto> ListVenuesAsync();
    Task<VenuesResponseDto> DeleteVenuesAsync(int[] ids);
    Task<VenueResponseDto> UpdateVenueAsync(int id, CreateVenueRequestDto dto);
}
