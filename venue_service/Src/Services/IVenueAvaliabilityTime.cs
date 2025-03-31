namespace venue_service.Src.Services
{
    public class IVenueAvaliabilityTime
    {
        Task<VenueAvailabilityTimeResponseDto> CreateVenueAvailabilityTimeAsync(CreateVenueAvailabilityTimeDto dto);
        Task<VenueAvailabilityTimeResponseDto> UpdateVenueAvailabilityTimeAsync(int id, UpdateVenueAvailabilityTimeDto dto);
        Task<bool> DeleteVenueAvailabilityTimeAsync(int id);
        Task<IVenueAvaliabilityTime> GetVenueAvaliabilityTimesByVenueIdAsync();
    }
}
