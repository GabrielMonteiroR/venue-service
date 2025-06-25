using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos.User;
using venue_service.Src.Exceptions;
using venue_service.Src.Interfaces.UserInterfaces;

namespace venue_service.Src.Services.User;

public class UserService : IUserService
{
    private readonly UserContext _userContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(UserContext context, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _userContext = context;
    }

    public async Task<UserResponseDto> GetUserInfoByIdAsync(int id)
    {
        try
        {
            var user = await _userContext.Users.FindAsync(id);
            if (user is null) throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with id {id} not found.");

            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                ProfileImage = user.ProfileImageUrl,
                RoleId = user.RoleId,
            };
        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving user information.", ex.Message);
        }
    }

    public async Task<UserResponseDto> UpdateUserInfoAsync(int id, UpdateUserDto userDto)
    {
        try
        {
            var user = await _userContext.Users.FindAsync(id);
            if (user is null) throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with id {id} not found.");

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.Phone = userDto.Phone;
            user.RoleId = userDto.RoleId;
            user.UpdatedAt = DateTime.UtcNow;

            var emailExists = await _userContext.Users.AnyAsync(u => u.Email == userDto.Email && u.Id != id);

            if (emailExists)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict, "Email already exists", $"The email {userDto.Email} is already in use by another user.");
            }
            var phoneExists = await _userContext.Users.AnyAsync(u => u.Phone == userDto.Phone && u.Id != id);
            if (phoneExists)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict, "Phone number already exists", $"The phone number {userDto.Phone} is already in use by another user.");
            }

            _userContext.Users.Update(user);
            await _userContext.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                RoleId = user.RoleId,
            };
        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while updating user information.", ex.Message);
        }
    }

    public async Task<UpdateUserProfileImageResponseDto> UpdateProfileImage(int userId, UpdateUserImageDto dto)
    {
        try
        {
            var user = await _userContext.Users.FindAsync(userId);
            if (user is null) throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with id {userId} not found.");

            user.ProfileImageUrl = dto.ImageUrl;

            await _userContext.SaveChangesAsync();

            var response = new UpdateUserProfileImageResponseDto
            {
                Id = user.Id,
                Message = "Profile image updated successfully",
                newProfileImageUrl = dto.ImageUrl
            };

            return response;

        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while updating user profile image.", ex.Message);
        }
    }

    public async Task<UserResponseDto> GetCurrentUser()
    {
        try
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized, "Unauthorized", "User is not authenticated.");
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid User ID", "User ID is not valid.");
            }

            var user = await _userContext.Users.FindAsync(userId);
            if (user is null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with id {userId} not found.");
            }

            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfileImage = user.ProfileImageUrl,
                Phone = user.Phone,
                RoleId = user.RoleId,
            };

        } catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving current user information.", ex.Message);
        }
    }
}
