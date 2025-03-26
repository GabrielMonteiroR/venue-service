using venue_service.Src.Dtos;

namespace venue_service.Src.Services
{
    public interface IVenueService
    {
        Task<VenueResponseDto> createVenueAsync(CreateVenueDto dto);
        Task<VenueResponseDto> updateVenueAsync(updateVenueDto dto);
        Task<>
    }
}
