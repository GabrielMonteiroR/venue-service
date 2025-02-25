namespace venue_service.Src.Models.Venue.Equipament
{
    public class VenueEquipament
    {
        public int Id { get; set }
        public string EquipamentName { get; set; }
        public EquipamentBrand equipamentBrand { get; set; }
        public EquipamentType equipamentType { get; set; }
        public int Quantity { get; set; }
    }
}
