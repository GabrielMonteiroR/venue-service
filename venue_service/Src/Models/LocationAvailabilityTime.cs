namespace venue_service.Src.Models
{
    //TODO RELATIONSHIP
    public class LocationAvailabilityTime
    {
        public VenueStatusEnum Status { get; set; }
        public decimal Price { get; set; }
        public DateTime Time { get; set; }
    }
}
