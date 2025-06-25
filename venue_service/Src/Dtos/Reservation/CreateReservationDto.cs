using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.Reservation;

public class CreateReservationDto
{
    [Required]
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [Required]
    [JsonPropertyName("venueId")]
    public int VenueId { get; set; }

    [Required]
    [JsonPropertyName("venueAvailabilityTimeId")]
    public int VenueAvailabilityTimeId { get; set; }

    [Required]
    [JsonPropertyName("paymentMethodId")]
    public int PaymentMethodId { get; set; }

    [Required]
    [JsonPropertyName("isPaid")]
    public bool IsPaid { get; set; }
}
