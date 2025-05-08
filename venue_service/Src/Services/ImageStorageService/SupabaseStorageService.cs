using venue_service.Src.Config;
using venue_service.Src.Services.ImageService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using venue_service.Src.Exceptions;
using System.Net;

namespace venue_service.Src.Services.ImageStorageService
{
    public class SupabaseStorageService : IStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly SupabaseStorageOptions _options;

        public SupabaseStorageService(HttpClient httpClient, SupabaseStorageOptions options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task<string?> UploadImageAsync(IFormFile file, string bucket, string path)
        {
            try
            {
                var requestUrl = $"{_options.Url}/storage/v1/object/{bucket}/{path}";

                using var streamContent = new StreamContent(file.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = streamContent
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return $"{_options.Url}/storage/v1/object/public/{bucket}/{path}";
            } catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "Internal Server Error", ex.Message);
            }
        }

        public async Task<bool> DeleteImageAsync(string bucket, string path)
        {
            var requestUri = $"{_options.Url}/storage/v1/object/{bucket}/{path}";

            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }


    }
}
