using System.Text.Json.Serialization;

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

        [JsonPropertyName("scheduleId")]
        public int ScheduleId { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("paymentMethodId")]
        public int PaymentMethodId { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("isPaid")]
        public bool? IsPaid { get; set; }

        [JsonPropertyName("paymentStatus")]
        public int PaymentStatus { get; set; }

        [JsonPropertyName("paidAt")]
        public DateTime? PaidAt { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
