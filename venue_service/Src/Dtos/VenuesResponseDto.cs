namespace venue_service.Src.Dtos
{
    public class VenuesResponseDto
    {
        public string Message { get; set; }
        public IEnumerable<VenueResponseDto> Data { get; set; }
    }
}
