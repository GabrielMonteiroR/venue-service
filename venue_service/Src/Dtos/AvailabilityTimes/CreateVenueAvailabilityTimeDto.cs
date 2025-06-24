namespace venue_service.Src.Dtos.AvailabilityTimes;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class CreateVenueAvailabilityTimeDto
{
    [Required]
    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [Required]
    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }

    [Required]
    [JsonPropertyName("venue_id")]
    public int VenueId { get; set; }

    [Required]
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    
    [Required]
    [JsonPropertyName("is_reserved")]
    public bool IsReserved { get; set; } = false;

    [JsonPropertyName("user_id")]
    public int? UserId { get; set; }
}
