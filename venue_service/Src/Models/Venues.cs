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
        public TimeSpan deletedAt { get; set; }
        public User Owner { get; set; }
        public VenueEquipaments equipaments { get; set; }
        public Sport AvaliableSports { get; set; }
        public VenueType Type { get; set; }


        //OR
        public LocationAvailability LocationAvailability { get; set; }
        //public VenueStatus Status { get; set; }
        //public Price Price { get; set; }
        //public AvaliableTime avaliableTimes { get; set; }



    }
}
