namespace venue_service.Src.Models
{
    //TODO RELATIONSHIP
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<EquipamentType> EquipamentTypes { get; set; } = new();
    }
}
