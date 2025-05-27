using venue_service.Src.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using venue_service.Src.Exceptions;
using System.Net;
using venue_service.Src.Interfaces.ImageStorageInterfaces;
using venue_service.Src.Dtos.ImageUpload;

namespace venue_service.Src.Services.ImageStorageService
{
    public class SupabaseStorageService : IStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly SupabaseStorageOptions _options;

        public SupabaseStorageService(HttpClient httpClient, IOptions<SupabaseStorageOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            Console.WriteLine($"🔧 Supabase URL carregada: {_options.Url}");

            if (string.IsNullOrWhiteSpace(_options.Url) || string.IsNullOrWhiteSpace(_options.ApiKey))
                throw new InvalidOperationException("Supabase URL ou API Key não configurada corretamente.");
        }

        public async Task<ImageUploadResponseDto> UploadImageAsync(IFormFile file, string bucket, string path)
        {
            try
            {
                var relativePath = $"/storage/v1/object/{bucket}/{path}";

                using var streamContent = new StreamContent(file.OpenReadStream());
                streamContent.Headers.ContentType =
                !string.IsNullOrWhiteSpace(file.ContentType) ? new MediaTypeHeaderValue(file.ContentType) : new MediaTypeHeaderValue("image/jpeg");


                var request = new HttpRequestMessage(HttpMethod.Post, relativePath)
                {
                    Content = streamContent
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

                var response = await _httpClient.SendAsync(request);

                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return null;
                ;

                return new ImageUploadResponseDto
                {
                    ImageUrl = $"{_options.Url}/storage/v1/object/public/{bucket}/{path}"
                };

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Erro ao enviar imagem para o Supabase", ex.Message);
            }
        }

        public async Task<bool> DeleteFileAsync(string bucket, string path)
        {
            var relativePath = $"/storage/v1/object/{bucket}/{path}";

            var request = new HttpRequestMessage(HttpMethod.Delete, relativePath);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public (string Bucket, string Path)? ParseSupabaseUrl(string url)
        {
            var parts = url.Split("/object/");
            if (parts.Length != 2) return null;

            var segments = parts[1].Split('/');
            var bucket = segments[0];
            var path = string.Join("/", segments.Skip(1));

            return (bucket, path);
        }

        public async Task<ImageUploadResponseDto> UploadProfileImageAsync(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var image = await UploadImageAsync(file, "profile-images", fileName);

            return new ImageUploadResponseDto
            {
                ImageUrl = image.ImageUrl
            };
        }

        public async Task<List<ImageUploadResponseDto>> UploadVenueImagesAsync(List<IFormFile> files)
        {
            var urls = new List<ImageUploadResponseDto>();

            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var url = await UploadImageAsync(file, "venue-images", fileName);
                if (url != null) urls.Add(url);
            }

            return urls;
        }
    }
}
