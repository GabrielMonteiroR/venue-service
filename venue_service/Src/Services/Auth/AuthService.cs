using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.Auth;
using venue_service.Src.Enums;
using venue_service.Src.Models.User;

namespace venue_service.Src.Services.Auth;

public class AuthService
{
    private readonly UserContext _userContext;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<UserEntity> _passwordHasher;

    public AuthService(UserContext context, IConfiguration configuration)
    {
        _userContext = context;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<UserEntity>();
    }

    public async Task<AuthResponseDto> RegisterOwnerAsync(RegisterOwnerRequestDto dto)
    {
        if (_userContext.Users.Any(u => u.Email == dto.Email))
            throw new Exception("Email already in use!");

        var user = new UserEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Cnpj = dto.Cnpj,
            Phone = dto.Phone,
            ProfileImageUrl = dto.Image,
            RoleId = (int)UserRoleEnum.OWNER, 
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        user.Password = _passwordHasher.HashPassword(user, dto.Password);

        _userContext.Users.Add(user);
        await _userContext.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = GenerateToken(user),
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            RoleId = user.RoleId,
            IsBanned = user.IsBanned,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }


    public async Task<AuthResponseDto> RegisterAthleteAsync(RegisterUserRequestDto dto)
    {
        if (_userContext.Users.Any(u => u.Email == dto.Email))
            throw new Exception("Email already in use!");

        var user = new UserEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            ProfileImageUrl = dto.Image,
            RoleId = (int)UserRoleEnum.ATHLETE,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        user.Password = _passwordHasher.HashPassword(user, dto.Password);

        _userContext.Users.Add(user);
        await _userContext.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = GenerateToken(user),
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            RoleId = user.RoleId,
            IsBanned = user.IsBanned,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public async Task<AuthResponseDto> Login(LoginRequestDto dto)
    {
        var user = _userContext.Users
            .Include(u => u.Role) 
            .FirstOrDefault(u => u.Email == dto.Email)
            ?? throw new Exception("Invalid user or password.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Invalid user or password.");

        return new AuthResponseDto
        {
            Token = GenerateToken(user),

            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            RoleId = user.RoleId,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    private string GenerateToken(UserEntity user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.RoleId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
