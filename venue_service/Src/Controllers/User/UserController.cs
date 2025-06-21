using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.User;
using venue_service.Src.Interfaces.ImageStorageInterfaces;
using venue_service.Src.Interfaces.ReservationInterfaces;
using venue_service.Src.Interfaces.UserInterfaces;
using venue_service.Src.Services.User;

namespace venue_service.Src.Controllers.User;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IReservationService _reservationService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfo(int id)
    {
        var userInfo = await _userService.GetUserInfoByIdAsync(id);
        return Ok(userInfo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserInfo(int id, [FromBody] UpdateUserDto userDto)
    {
        var updatedUser = await _userService.UpdateUserInfoAsync(id, userDto);
        return Ok(updatedUser);
    }

    [HttpPatch("{id}/profile-image")]
    public async Task<IActionResult> UpdateProfileImage(int id, [FromBody] UpdateUserImageDto dto)
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

    [HttpGet("{userId}/next-reservation")]
    public async Task<IActionResult> GetNextReservation(int userId)
    {
        var response = await _reservationService.GetNextUserReservationAsync(userId);
        return Ok(response);
    }
}

