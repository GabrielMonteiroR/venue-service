using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.User;
using venue_service.Src.Interfaces.ImageStorageInterfaces;
using venue_service.Src.Interfaces.UserInterfaces;
using venue_service.Src.Services.User;

namespace venue_service.Src.Controllers.User
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IStorageService _storageService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfo([FromQuery] int id)
        {
            var userInfo = await _userService.GetUserInfoByIdAsync(id);
            return Ok(userInfo);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo([FromQuery] int id, [FromBody] UpdateUserDto userDto)
        {
            var updatedUser = await _userService.UpdateUserInfoAsync(id, userDto);
            return Ok(updatedUser);
        }

        [HttpPatch("profile-image")]
        public async Task<IActionResult> UpdateProfileImage([FromQuery] int id, [FromBody] UpdateUserImageDto dto)
        {
            var updatedUser = await _userService.UpdateProfileImage(id, dto);
            return Ok(updatedUser);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var response = await _userService.GetCurrentUser();
            return Ok(response);
        }
    }
}
