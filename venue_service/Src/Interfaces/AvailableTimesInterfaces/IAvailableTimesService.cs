using venue_service.Src.Dtos.AvailabilityTimes;

namespace venue_service.Src.Interfaces.AvailableTimesInterfaces;

public interface IAvailableTimesService
{
    Task<VenueAvailabilityTimeDto> CreateVenueAvailabilityTime(CreateVenueAvailabilityTimeDto requestDto);
    Task<VenueAvailabilityTimeResponseDto> GetVenueAvailabilityTimes();
    Task<VenueAvailabilityTimeDto> GetVenueAvailabilityTimeById(int id);
    Task<VenueAvailabilityTimeDto> UpdateVenueAvailabilityTime(int availabilityTimeId, UpdateVenueAvailabilityTimeDto requestDto);
    Task<DeleteVenueAvailabilityTimeDto> DeleteVenueAvailabilityTime(int id);
    Task<VenueAvailabilityTimeResponseDto> GetAvailabilityTimesByVenueId(int venueId, bool? isReserved = null);
}