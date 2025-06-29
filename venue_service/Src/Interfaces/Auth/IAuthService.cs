using venue_service.Src.Dtos.Auth;

namespace venue_service.Src.Interfaces.Auth;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterOwnerAsync(RegisterOwnerRequestDto dto);
    Task<AuthResponseDto> RegisterAthleteAsync(RegisterUserRequestDto dto);
    Task<AuthResponseDto> Login(LoginRequestDto dto);
    Task<UniqueValidatorDto> ValidateUniqueFieldsAsync(UniqueValidatorDto dto);
}
