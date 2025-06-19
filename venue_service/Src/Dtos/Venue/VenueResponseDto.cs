using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.Venue;

public class VenueResponseDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("street")]
    public string Street { get; set; }

    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("complement")]
    public string? Complement { get; set; }

    [JsonPropertyName("neighborhood")]
    public string Neighborhood { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("postal_code")]
    public string PostalCode { get; set; }

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
}
