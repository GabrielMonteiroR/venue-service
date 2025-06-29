using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.Auth;
using venue_service.Src.Exceptions;
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
    public async Task<IActionResult> RegisterAthleteAsync([FromBody] RegisterUserRequestDto dto)
    {
        try
        {
            var response = await _authService.RegisterAthleteAsync(dto);
            return Ok(response);
        }
        catch (ConflictException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro inesperado: " + ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("register-owner")]
    public async Task<IActionResult> RegisterOwner([FromBody] RegisterOwnerRequestDto dto)
    {
        try
        {
            var response = await _authService.RegisterOwnerAsync(dto);
            return Ok(response);
        }
        catch (ConflictException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro inesperado: " + ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
            var response = await _authService.Login(dto);
            return Ok(response);
    }

}
