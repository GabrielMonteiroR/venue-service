using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.User;
using venue_service.Src.Services;
using venue_service.Src.Services.ImageService;

namespace venue_service.Src.Controllers.User
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _profileService;
        private readonly IStorageService _storageService;

        public UserController(UserService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfo([FromQuery] int id)
        {
            var userInfo = await _profileService.GetUserInfoByIdAsync(id);
            return Ok(userInfo);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo([FromQuery] int id, [FromBody] UpdateUserDto userDto)
        {
            var updatedUser = await _profileService.UpdateUserInfoAsync(id, userDto);
            return Ok(updatedUser);
        }

        [HttpPatch("profile-image")]
        public async Task<IActionResult> UpdateProfileImage([FromQuery] int id, [FromBody] UpdateUserImageDto dto)
        {
            var updatedUser = await _profileService.UpdateProfileImage(id, dto);
            return Ok(updatedUser);
        }

    }
}
