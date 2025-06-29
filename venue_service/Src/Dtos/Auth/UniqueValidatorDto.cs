using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.Auth
{
    public class UniqueValidatorDto
    {
        [JsonPropertyName("emailExists")]
        public bool EmailExists { get; set; }

        [JsonPropertyName("cpfExists")]
        public bool CpfExists { get; set; }

        [JsonPropertyName("phoneExists")]
        public bool PhoneExists { get; set; }

    }
}
