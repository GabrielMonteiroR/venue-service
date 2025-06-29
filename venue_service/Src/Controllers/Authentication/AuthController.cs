using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.Auth;
using venue_service.Src.Services.Auth;

namespace venue_service.Src.Controllers.Authentication;

[Authorize]
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register-athlete")]
    public async Task<IActionResult> RegisterAthlete([FromBody] RegisterUserRequestDto dto)
    {
        var response = await _authService.RegisterAthleteAsync(dto);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("register-owner")]
    public async Task<IActionResult> RegisterOwner([FromBody] RegisterOwnerRequestDto dto)
    {
        var response = await _authService.RegisterOwnerAsync(dto);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
            var response = await _authService.Login(dto);
            return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("validate-unique")]
    public async Task<IActionResult> ValidateUnique([FromBody] UniqueValidatorDto dto)
    {
        var response = await _authService.ValidateUniqueFieldsAsync(dto);
        return Ok(response);
    }
}
