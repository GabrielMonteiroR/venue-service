namespace venue_service.Src.Dtos
{
    public class VenueAvailabilityTimeResponseDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VenueId { get; set; }
        public decimal Price { get; set; }
    }
}
