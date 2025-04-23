using venue_service.Src.Dtos;

namespace venue_service.Src.Services
{
    public interface IVenueType
    {
        Task<VenueTypesResponseDto> GetAllVenueTypes();
    }
}
