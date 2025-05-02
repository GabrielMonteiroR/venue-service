using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos;
using venue_service.Src.Services;

namespace venue_service.Src.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ProfileService _profileService;

        public UserController(ProfileService profileService)
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
        public async Task<IActionResult> UpdateUserInfo([FromQuery] int id, [FromBody] UserResponseDto userDto)
        {
            var updatedUser = await _profileService.UpdateUserInfoAsync(id, userDto);
            return Ok(updatedUser);
        }


    }
}
