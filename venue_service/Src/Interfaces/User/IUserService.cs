using venue_service.Src.Dtos.User;

namespace venue_service.Src.Interfaces.User
{
    public interface IUserService
    {
        Task<UserResponseDto> GetUserInfoByIdAsync(int id);
        Task<UserResponseDto> UpdateUserInfoAsync(int id, UpdateUserDto userDto);
        Task<UpdateUserProfileImageResponseDto> UpdateProfileImage(int userId, UpdateUserImageDto dto);
    }
}
