using System.Text.Json.Serialization;

namespace venue_service.Src.Models.AvailabilityTimes;

public class UpdateVenueAvailabilityTimeDto
{
    [JsonPropertyName("start_date")]
    public DateTime? StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime? EndDate { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("is_reserved")]
    public bool? IsReserved { get; set; }

    [JsonPropertyName("user_id")]
    public int? UserId { get; set; }
}

