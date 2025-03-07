namespace venue_service.Src.Models
{
    public class Equipament
    {
        public int Id { get; set; }
        public int EquipamentTypeId { get; set; }
        public EquipamentType EquipamentType { get; set; }
        public int VenueId { get; set; }
        public Venue Venue { get; set; }
        public bool IsAvailable { get; set; }
    }
}
