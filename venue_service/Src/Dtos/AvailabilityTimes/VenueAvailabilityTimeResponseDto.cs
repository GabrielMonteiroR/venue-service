namespace venue_service.Src.Dtos.AvailabilityTimes
{
    public class VenueAvailabilityTimeResponseDto
    {
        public string Message { get; set; }
        public List<VenueAvailabilityTimeDto> Data { get; set; }
    }
}
