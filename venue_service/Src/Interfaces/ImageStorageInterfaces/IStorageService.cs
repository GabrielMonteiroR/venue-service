using venue_service.Src.Dtos.ImageUpload;

namespace venue_service.Src.Interfaces.ImageStorageInterfaces;

public interface IStorageService
{
    Task<ImageUploadResponseDto> UploadProfileImageAsync(IFormFile file);
    Task<List<ImageUploadResponseDto>> UploadVenueImagesAsync(List<IFormFile> files);
    Task<bool> DeleteFileAsync(string bucket, string path);
    (string Bucket, string Path)? ParseSupabaseUrl(string url);
    Task<ImageUploadResponseDto?> UploadImageAsync(IFormFile file, string bucket, string path);
}