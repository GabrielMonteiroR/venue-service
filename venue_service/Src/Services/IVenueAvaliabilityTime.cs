using venue_service.Src.Dtos;

namespace venue_service.Src.Services
{
    public interface IVenueAvaliabilityTime
    {
        Task<VenueAvailabilityTimeResponseDto> CreateVenueAvailabilityTimeAsync(CreateVenueAvaliabilityDto dto);
        //Task<VenueAvailabilityTimeResponseDto> UpdateVenueAvailabilityTimeAsync(int id, UpdateVenueAvailabilityTimeDto dto);
        Task<bool> DeleteVenueAvailabilityTimeAsync(int id);
        Task<IVenueAvaliabilityTime> GetVenueAvaliabilityTimesByVenueIdAsync();
    }
}
