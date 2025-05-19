using System;

namespace venue_service.Src.Dtos.User
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImage { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }
        public bool IsBanned { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
