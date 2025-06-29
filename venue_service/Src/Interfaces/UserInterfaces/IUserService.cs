﻿using venue_service.Src.Dtos.User;

namespace venue_service.Src.Interfaces.UserInterfaces;

public interface IUserService
{
    Task<UserResponseDto> GetUserInfoByIdAsync(int id);
    Task<UserResponseDto> UpdateUserInfoAsync(int id, UpdateUserDto userDto);
    Task<UpdateUserProfileImageResponseDto> UpdateProfileImage(int userId, UpdateUserImageDto dto);
    Task<UserResponseDto> GetCurrentUser();
}
