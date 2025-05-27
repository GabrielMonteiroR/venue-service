using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.ImageUpload
{
    public class ImageUploadResponseDto
    {
        [JsonPropertyName("Image")]
        public string Image { get; set; }
    }
}
