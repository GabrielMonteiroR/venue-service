using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.Venue;

public class UpdateVenueRequestDto
{
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

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("allow_local_payment")]
    public bool AllowLocalPayment { get; set; }

    [JsonPropertyName("venue_type_id")]
    public int VenueTypeId { get; set; }

    [JsonPropertyName("rules")]
    public string Rules { get; set; }
}
