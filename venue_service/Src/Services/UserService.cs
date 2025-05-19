using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos;
using venue_service.Src.Exceptions;

namespace venue_service.Src.Services
{
    public class UserService
    {
        private readonly UserContext _userContext;

        public UserService(UserContext context)
        {
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
                    ProfileImage = user.ProfileImageUrl,
                    Phone = user.Phone,
                    RoleId = user.RoleId,
                    IsBanned = user.IsBanned
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
                user.IsBanned = userDto.IsBanned;
                user.UpdatedAt = DateTime.UtcNow;

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
                    IsBanned = user.IsBanned
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
    }
}
