namespace venue_service.Src.Dtos.Auth;

public class AuthResponseDto
{
    public string Token { get; set; }

    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public int RoleId { get; set; }
    public string RoleName { get; set; }

    public bool IsBanned { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
