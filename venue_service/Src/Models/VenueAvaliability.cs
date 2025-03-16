namespace venue_service.Src.Models.Venue
{
    public class VenueAvaliability
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public Venue Venue { get; set; } = null!;
        public LocationAvailability LocationAvailability { get; set; }
    }
}
