namespace venue_service.Src.Models;

public class Equipament
{
    public int Id { get; set; }
    public bool IsAvailable { get; set; }

    public List<Venue> Venues { get; set; }


}
