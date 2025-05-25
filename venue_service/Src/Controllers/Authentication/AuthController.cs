using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.Auth;
using venue_service.Src.Services.Auth;

namespace venue_service.Src.Controllers.Authentication;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register-athlete")]
    public async Task<IActionResult> RegisterAthlete([FromBody] RegisterUserRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _authService.RegisterAthleteAsync(dto);
        return Ok(response);
    }

    [HttpPost("register-owner")]
    public async Task<IActionResult> RegisterOwner([FromBody] RegisterOwnerRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _authService.RegisterOwnerAsync(dto);
        return Ok(response);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        try
        {
            var response = await _authService.Login(dto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
