namespace venue_service.Src.Dtos;

public class UpdateVenueImageDto
{
    public int VenueId { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}
