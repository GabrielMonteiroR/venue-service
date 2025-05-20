using System.ComponentModel.DataAnnotations;

namespace venue_service.Src.Dtos.Auth;

public class AuthResponseDto
{
    [Required]
    public string Token { get; set; }

    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; }

    [Phone]
    [StringLength(20)]
    public string Phone { get; set; }

    [Required]
    public int RoleId { get; set; }

    [Required]
    [StringLength(100)]
    public string RoleName { get; set; }

    [Required]
    public bool IsBanned { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }
}
