namespace venue_service.Src.Dtos
{
    public class UpdateVenueImageResponseDto
    {
        public int VenueId { get; set; }
        public List<string> NewImageUrls { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }
}
