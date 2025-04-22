namespace venue_service.Src.Dtos
{
    public class VenueAvaliabilityTimesResponseDto
    {
        public string Message { get; set; };
        public List<VenueAvailabilityTimeResponseDto> venueAvailabilityTimeResponseDtos { get; set; }
    }
}
