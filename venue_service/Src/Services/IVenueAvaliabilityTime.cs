using venue_service.Src.Dtos.Venue;
using venue_service.Src.Models;

namespace venue_service.Src.Services
{
    public interface IVenueAvaliabilityTime
    {
        Task<VenueAvailabilityTimeResponseDto> CreateVenueAvailabilityTimeAsync(CreateVenueAvaliabilityDto dto);
        Task<VenueAvaliabilityTimesResponseDto> ListAvaliableTimesByVenue(int venueId);
        Task<VenueAvailabilityTime> UpdateAvaliabilityTime(int id, UpdateVenueAvaliabilityDto newTimeDto);
        Task<bool> DeleteVenueAvailabilityTimeAsync(int id);
        Task<VenueAvailabilityTimeResponseDto> AssignAvaliableTime(int userId);
    }
}
