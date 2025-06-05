using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.Sports
{
    public class SportsResponseDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public List<SportResponseDto> Data { get; set; }
    }
}
