using venue_service.Src.Dtos.Venue;

namespace venue_service.Src.Services
{
    public interface IVenueType
    {
        Task<VenueTypesResponseDto> GetAllVenueTypes();
    }
}
