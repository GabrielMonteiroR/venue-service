using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.Venue
{
    public class CreateVenueRequestDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("image_urls")]
        public List<string> ImageUrls { get; set; } = new();

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("allow_local_payment")]
        public bool AllowLocalPayment { get; set; }

        [JsonPropertyName("venue_type_id")]
        public int VenueTypeId { get; set; }

        [JsonPropertyName("rules")]
        public string Rules { get; set; }

        [JsonPropertyName("owner_id")]
        public int OwnerId { get; set; }
    }
}
