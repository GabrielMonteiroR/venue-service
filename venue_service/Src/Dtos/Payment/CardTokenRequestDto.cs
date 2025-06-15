using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.Payment
{
    public class CardTokenRequestDto
    {
        [JsonPropertyName("card_number")]
        public string CardNumber { get; set; }

        [JsonPropertyName("expiration_month")]
        public int ExpirationMonth { get; set; }

        [JsonPropertyName("expiration_year")]
        public int ExpirationYear { get; set; }

        [JsonPropertyName("security_code")]
        public string SecurityCode { get; set; }

        [JsonPropertyName("cardholder")]
        public CardholderDto Cardholder { get; set; }

        public class CardholderDto
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("identification")]
            public IdentificationDto Identification { get; set; }
        }

        public class IdentificationDto
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("number")]
            public string Number { get; set; }
        }
    }
}
