using Microsoft.AspNetCore.Mvc;
using venue_service.Src.Dtos.Auth;
using venue_service.Src.Services;

namespace venue_service.Src.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _authService.RegisterAsync(dto);
        return Ok(response);
    }


    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto dto)
    {
        try
        {
            var response = _authService.Login(dto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
