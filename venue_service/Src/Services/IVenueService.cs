using venue_service.Src.Dtos;

namespace venue_service.Src.Services;

public interface IVenueService
{
    Task<VenueResponseDto> CreateVenueAsync(CreateVenueDto dto);
    Task<VenuesResponseDto> ListVenuesAsync();
}
