namespace venue_service.Src.Models
{
    public class Venues
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public VenueContactInfo ContactInfo { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public bool AllowLocalPayment { get; set; }
        public List<VenueImage> VenueImages { get; set; } = new List<VenueImage>();
        public TimeSpan CreatedAt { get; set; }
        public TimeSpan UpdatedAt { get; set; }
        public TimeSpan DeletedAt { get; set; }
        public User Owner { get; set; }
        public List<Sport> AvaliableSports { get; set; } = new List<Sport>();
        public VenueType TypeOfVenue { get; set; }
        public List<LocationAvailabilityTime> LocationAvailability { get; set; } = new List<LocationAvailabilityTime>();

        public List<Equipament> Equipaments { get; set; } = new List<Equipament>();
    }
}
