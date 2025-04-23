namespace venue_service.Src.Dtos
{
    public class VenueTypesResponseDto
    {
        public string Message { get; set; }
        public List<VenueTypeResponseDto> venueTypes { get; set; }
    }
}
