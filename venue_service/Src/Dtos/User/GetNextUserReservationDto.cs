using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.User
{
    public class GetNextUserReservationDto
    {
        [JsonPropertyName("reservationId")]
        public int ReservationId { get; set; }

        [JsonPropertyName("venueId")]
        public int VenueId { get; set; }

        [JsonPropertyName("venueName")]
        public string VenueName { get; set; }

        [JsonPropertyName("venueAddress")]
        public string VenueAddress { get; set; }

        [JsonPropertyName("scheduleId")]
        public int ScheduleId { get; set; }

        [JsonPropertyName("scheduleDate")]
        public SchedulesEntity Schedule { get; set; }

        [JsonPropertyName("venueImage")]
        public string venueImage { get; set; }

        [JsonPropertyName("reservationStatus")]
        public string reservationStatus { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("paymentMethodId")]
        public int PaymentMethodId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
