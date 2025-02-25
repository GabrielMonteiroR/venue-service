namespace venue_service.Src.Models
{
    public class User_Venue
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int VenueId { get; set; }
        public Venues Venue { get; set; }
    }
}
