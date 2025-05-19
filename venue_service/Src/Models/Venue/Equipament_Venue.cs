namespace venue_service.Src.Models.Venue
{
    public class Equipament_Venue
    {
        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        public int EquipamentId { get; set; }
        public VenueEquipament Equipament { get; set; }
    }
}
