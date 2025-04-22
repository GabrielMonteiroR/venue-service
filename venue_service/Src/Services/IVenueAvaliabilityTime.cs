using venue_service.Src.Dtos;

namespace venue_service.Src.Services
{
    public interface IVenueAvaliabilityTime
    {
        Task<VenueAvailabilityTimeResponseDto> CreateVenueAvailabilityTimeAsync(CreateVenueAvaliabilityDto dto);
        Task<VenueAvaliabilityTimesResponseDto> ListAvaliableTimesByVenue(int venueId);
        Task<bool> DeleteVenueAvailabilityTimeAsync(int id);
    }
}
