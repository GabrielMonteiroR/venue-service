using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using venue_service.Src.Models.Venue;

namespace venue_service.Src.Dtos.Venue
{
    public class VenueResponseDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("venue_type_name")]
        public string venueTypeName { get; set; }

        [JsonPropertyName("owner_name")]
        public string OwnerName { get; set; }

        [JsonPropertyName("sports")]
        public List<string> Sports { get; set; } = new();

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("allow_local_payment")]
        public bool AllowLocalPayment { get; set; }

        [JsonPropertyName("image_urls")]
        public List<string> ImageUrls { get; set; } = new();

        [JsonPropertyName("venue_type_id")]
        public int VenueTypeId { get; set; }

        [JsonPropertyName("rules")]
        public string Rules { get; set; }

        [JsonPropertyName("owner_id")]
        public int OwnerId { get; set; }

        [JsonPropertyName("venue_avaliability_times")]
        public List<VenueAvailabilityTimeResponseDto> venueAvaliabilityTimes { get; set; }

        [JsonPropertyName("venue_times")]
        public List<string> VenueTimes { get; set; } = new();
    }
}
