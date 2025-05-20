namespace venue_service.Src.Iterfaces.ImageStorage
{
    public interface IStorageService
    {
        Task<string?> UploadProfileImageAsync(IFormFile file);
        Task<List<string>> UploadVenueImagesAsync(List<IFormFile> files);
        Task<bool> DeleteFileAsync(string bucket, string path);
        (string Bucket, string Path)? ParseSupabaseUrl(string url);
        Task<string?> UploadImageAsync(IFormFile file, string bucket, string path);
    }

}