namespace venue_service.Src.Models.Venue
{
    public class Equipament_VenueEntity
    {
        public int VenueId { get; set; }
        public VenueEntity Venue { get; set; }

        public int EquipamentId { get; set; }
        public VenueEquipamentEntity Equipament { get; set; }
    }
}
