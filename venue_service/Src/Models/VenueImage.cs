namespace venue_service.Src.Models.Venue
{
    public class VenueImage
    {
        public int Id { get; set; }
        public string ImageURL { get; set; }

        public int VenueId { get; set; }
        public Venue Venue { get; set; } = null!;
    }
}
