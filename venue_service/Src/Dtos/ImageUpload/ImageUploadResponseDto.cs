using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.ImageUpload
{
    public class ImageUploadResponseDto
    {
        [JsonPropertyName("image")]
        public string Image { get; set; }
    }
}
