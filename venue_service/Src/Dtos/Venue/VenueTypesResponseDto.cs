namespace venue_service.Src.Dtos.Venue
{
    public class VenueTypesResponseDto
    {
        public string Message { get; set; }
        public List<VenueTypeResponseDto> venueTypesList { get; set; }
    }
}
