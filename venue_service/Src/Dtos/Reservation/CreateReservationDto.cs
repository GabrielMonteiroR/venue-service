using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.Reservation;

public class CreateReservationDto
{
    [Required]
    [JsonPropertyName("venueId")]
    public int VenueId { get; set; }

    [Required]
    [JsonPropertyName("scheduleId")]
    public int ScheduleId { get; set; }

    [Required]
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [Required]
    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount { get; set; }

    [Required]
    [Range(1, 3, ErrorMessage = "Método de pagamento inválido")]
    [JsonPropertyName("paymentMethodId")]
    public int PaymentMethodId { get; set; }

    [StringLength(255, MinimumLength = 1)]
    [JsonPropertyName("cardToken")]
    public string? CardToken { get; set; }

    [EmailAddress]
    [StringLength(255)]
    [JsonPropertyName("userEmail")]
    public string? UserEmail { get; set; }
}
