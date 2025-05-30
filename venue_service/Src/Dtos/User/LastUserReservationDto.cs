using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.User
{
    public class LastUserReservationDto
    {
        [JsonPropertyName("reservationId")]
        public int reservationId { get; set; }

        [JsonPropertyName("venueId")]
        public int venueId { get; set; }

        [JsonPropertyName("venueName")]
        public string venueName { get; set; }

        [JsonPropertyName("venueAddress")]
        public string venueAddress { get; set; }

        [JsonPropertyName("scheduleId")]
        public int ScheduleId { get; set; }

        [JsonPropertyName("venueImage")]
        public string venueImage { get; set; }

        [JsonPropertyName("reservationStatus")]
        public string reservationStatus { get; set; }
    }
}
