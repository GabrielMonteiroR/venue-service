using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.AvailabilityTimes;

public class VenueAvailabilityTimeDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("venue_id")]
    public int VenueId { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("is_reserved")]
    public bool IsReserved { get; set; }

    [JsonPropertyName("user_id")]
    public int? UserId { get; set; }
}
