using venue_service.Src.Dtos.Venue;

namespace venue_service.Src.Interfaces.Venue
{
    public interface IVenueType
    {
        Task<VenueTypesResponseDto> GetAllVenueTypes();
    }
}
