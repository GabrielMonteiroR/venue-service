namespace venue_service.Src.Models
{
    public class Venue
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
        public List<VenueImage> VenueImages { get; set; } = new();
        public List<Sport> AvaliableSports { get; set; } = new();
        public List<Equipament> Equipaments { get; set; } = new();
        public VenueTypeEnum TypeOfVenue { get; set; }
        public List<LocationAvailabilityTime> LocationAvailability { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
