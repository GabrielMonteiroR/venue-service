using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.User;

public class PartialUserResponseDto
{
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("profile_image_url")]
    public string? ProfileImage { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }
}
