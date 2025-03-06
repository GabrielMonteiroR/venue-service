namespace venue_service.Src.Models
{
    public class Equipament
    {
        public int Id { get; set; }
        public EquipamentType EquipamentType { get; set; } 
        public Venues Venue { get; set; } 
        public bool IsAvailable { get; set; } 
    }
}
