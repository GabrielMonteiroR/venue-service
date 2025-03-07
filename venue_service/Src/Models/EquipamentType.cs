namespace venue_service.Src.Models
{
    public class EquipamentType
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public int SportId { get; set; }
        public Sport Sport { get; set; }

        public List<Equipament> Equipaments { get; set; }
    }
}
