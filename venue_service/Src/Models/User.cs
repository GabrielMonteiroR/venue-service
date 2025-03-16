
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("first_name")]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("last_name")]
    public string LastName { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [Column("password")]
    public string Password { get; set; }

    [MaxLength(20)]
    [Column("phone")]
    public string Phone { get; set; }

    [ForeignKey("RoleId")]
    public Role Role { get; set; }
    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("is_banned")]
    public bool IsBanned { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public ICollection<User_Venue> UserVenues { get; set; }
}
