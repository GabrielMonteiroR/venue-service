using venue_service.Src.Dtos.Auth;

namespace venue_service.Src.Interfaces.Auth;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterOwnerAsync(RegisterOwnerRequestDto dto);
    Task<AuthResponseDto> RegisterAthleteAsync(RegisterUserRequestDto dto);
    AuthResponseDto Login(LoginRequestDto dto);
}
