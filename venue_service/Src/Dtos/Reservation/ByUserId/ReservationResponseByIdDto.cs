using System.Text.Json.Serialization;
using venue_service.Src.Dtos.AvailabilityTimes;
using venue_service.Src.Dtos.User;
using venue_service.Src.Dtos.Venue;

namespace venue_service.Src.Dtos.Reservation.ByUserId
{
    public class ReservationResponseByIdDto
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

        [JsonPropertyName("user")]
        public PartialUserResponseDto Locator { get; set; }

        public VenueResponseDto Venue { get; set; }

        [JsonPropertyName("paymentMethodId")]
        public int PaymentMethodId { get; set; }

        [JsonPropertyName("isPaid")]
        public bool? IsPaid { get; set; }

    }
}