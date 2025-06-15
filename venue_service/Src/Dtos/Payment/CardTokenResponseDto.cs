using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.Payment;

public class CardTokenResponseDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("public_key")]
    public string PublicKey { get; set; }

    [JsonPropertyName("first_six_digits")]
    public string FirstSixDigits { get; set; }

    [JsonPropertyName("expiration_month")]
    public int ExpirationMonth { get; set; }

    [JsonPropertyName("expiration_year")]
    public int ExpirationYear { get; set; }

    [JsonPropertyName("last_four_digits")]
    public string LastFourDigits { get; set; }

    [JsonPropertyName("cardholder")]
    public CardholderDto Cardholder { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("date_created")]
    public DateTime DateCreated { get; set; }

    [JsonPropertyName("date_last_updated")]
    public DateTime DateLastUpdated { get; set; }

    [JsonPropertyName("date_due")]
    public DateTime DateDue { get; set; }

    [JsonPropertyName("luhn_validation")]
    public bool LuhnValidation { get; set; }

    [JsonPropertyName("live_mode")]
    public bool LiveMode { get; set; }

    [JsonPropertyName("require_esc")]
    public bool RequireEsc { get; set; }

    [JsonPropertyName("card_number_length")]
    public int CardNumberLength { get; set; }

    [JsonPropertyName("security_code_length")]
    public int SecurityCodeLength { get; set; }

    [JsonPropertyName("trunc_card_number")]
    public string TruncCardNumber { get; set; }
}

public class CardholderDto
{
    [JsonPropertyName("identification")]
    public IdentificationDto Identification { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class IdentificationDto
{
    [JsonPropertyName("number")]
    public string Number { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
