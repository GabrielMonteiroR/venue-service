using venue_service.Src.Dtos.Venue;
using venue_service.Src.Models.Venue;

namespace venue_service.Src.Interfaces.Venue
{
    public interface IVenueAvaliabilityTime
    {
        Task<VenueAvailabilityTimeResponseDto> CreateVenueAvailabilityTimeAsync(CreateVenueAvaliabilityDto dto);
        Task<VenueAvaliabilityTimesResponseDto> ListAvaliableTimesByVenue(int venueId);
        Task<VenueAvailabilityTimeEntity> UpdateAvaliabilityTime(int id, UpdateVenueAvaliabilityDto newTimeDto);
        Task<bool> DeleteVenueAvailabilityTimeAsync(int id);
        Task<VenueAvailabilityTimeResponseDto> AssignAvaliableTime(int userId);
        Task<bool> IsThisTimeAvailableToBook(int id);
    }
}
