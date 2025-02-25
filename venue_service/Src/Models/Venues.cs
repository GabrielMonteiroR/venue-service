using System;

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
        public VenueImage VenueImages { get; set; }
        public TimeSpan CreatedAt { get; set; }
        public TimeSpan UpdatedAt { get; set; }
        public TimeSpan DeletedAt { get; set; }
        public User Owner { get; set; }
        public VenueEquipaments Equipaments { get; set; }
        public Sport AvaliableSports { get; set; }
        public VenueType Type { get; set; }
        public VenueAvaliability VenueAvaliability { get; set; }

        public ICollection<UserVenue> UserVenues { get; set; }
    }
}
