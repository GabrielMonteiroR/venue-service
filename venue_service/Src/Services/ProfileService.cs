using System.Net;
using venue_service.Src.Contexts;
using venue_service.Src.Dtos;
using venue_service.Src.Exceptions;

namespace venue_service.Src.Services
{
    public class ProfileService
    {
        private readonly DatabaseContext _context;

        public ProfileService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDto> GetUserInfoByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user is null) throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with id {id} not found.");

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
            } catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError, "An error occurred while retrieving user information.", ex.Message);
            }
        }

    public async Task<UserResponseDto> UpdateUserInfoAsync(int id, UserResponseDto userDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user is null) throw new HttpResponseException(HttpStatusCode.NotFound, "User not found", $"User with id {id} not found.");
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.Email = userDto.Email;
                user.Phone = userDto.Phone;
                user.RoleId = userDto.RoleId;
                user.IsBanned = userDto.IsBanned;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

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
    }
}
