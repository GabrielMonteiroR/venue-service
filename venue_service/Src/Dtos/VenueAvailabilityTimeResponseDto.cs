namespace venue_service.Src.Dtos
{
    public class VenueAvailabilityTimeResponseDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime VenueId { get; set; }
        public double Price { get; set; }
    }
}
