namespace venue_service.Src.Services.ImageService
{
    public interface IStorageService
    {
        Task<string> UploadImageAsync(IFormFile file, string bucket, string path);
        Task<bool> DeleteImageAsync(string bucket, string path);
        Task ParseSupabaseUrlAsync(string url);
    }
}
