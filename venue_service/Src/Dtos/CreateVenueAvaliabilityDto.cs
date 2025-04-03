namespace venue_service.Src.Dtos
{
    public class CreateVenueAvaliabilityDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VenueId { get; set; }
        public decimal Price { get; set; }
    }
}
