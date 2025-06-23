namespace venue_service.Src.Models.AvailabilityTimes;

using System.Text.Json.Serialization;

public class CreateVenueAvailabilityTimeDto
{
    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("venue_id")]
    public int VenueId { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("is_reserved")]
    public bool IsReserved { get; set; } = false;

    [JsonPropertyName("user_id")]
    public int? UserId { get; set; }
}
