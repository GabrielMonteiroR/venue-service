using System.Text.Json.Serialization;
using venue_service.Src.Dtos.AvailabilityTimes;

namespace venue_service.Src.Dtos.Reservation
{
    public class ReservationResponseDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("venueId")]
        public int VenueId { get; set; }

        [JsonPropertyName("venueAvailabilityTimeId")]
        public int VenueAvailabilityTimeId { get; set; }

        [JsonPropertyName("venueAvailabilityTime")]
        public VenueAvailabilityTimeDto VenueAvailabilityTime { get; set; }

        [JsonPropertyName("paymentMethodId")]
        public int PaymentMethodId { get; set; }

        [JsonPropertyName("isPaid")]
        public bool? IsPaid { get; set; }

    }
}