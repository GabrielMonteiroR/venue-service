namespace venue_service.Src.Models
{
    public class UserVenue
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int VenueId { get; set; }
        public Venues Venue { get; set; }
    }
}
