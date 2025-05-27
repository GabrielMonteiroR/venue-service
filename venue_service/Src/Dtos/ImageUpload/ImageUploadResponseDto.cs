using System.Text.Json.Serialization;

namespace venue_service.Src.Dtos.ImageUpload
{
    public class ImageUploadResponseDto
    {
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
