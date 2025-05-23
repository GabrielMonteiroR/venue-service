using venue_service.Src.Dtos.Venue;

namespace venue_service.Src.Interfaces.VenueInterfaces;

public interface IVenueType
{
    Task<VenueTypesResponseDto> GetAllVenueTypes();
}
