using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Interfaces.ImageStorage;

namespace venue_service.Src.Controllers.ImageUpload
{

    [ApiController]
    [Route("upload-images")]
    public class UploadController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public UploadController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost("profile-image")]
        public async Task<IActionResult> UploadProfileImage([FromForm] IFormFile image)
        {
            var url = await _storageService.UploadProfileImageAsync(image);
            return Ok(new { imageUrl = url });
        }

        [HttpPost("venue-images")]
        public async Task<IActionResult> UploadVenueImages([FromForm] List<IFormFile> images)
        { 
            var urls = await _storageService.UploadVenueImagesAsync(images);
            return Ok(new { imageUrls = urls });
        }
    }
}
