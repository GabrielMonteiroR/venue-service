using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using venue_service.Src.Models.Venue;

namespace venue_service.Src.Models.User
{
    [Table("users")]
    public class UserEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("last_name")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [Column("phone")]
        public string Phone { get; set; }

        [Column("cpf")]
        public string? Cpf { get; set; }

        [Column("cnpj")]
        public string? Cnpj { get; set; }

        [Column("profile_image_url")]
        public string? ProfileImageUrl { get; set; }

        [Column("is_banned")]
        public bool IsBanned { get; set; }

        [Required]
        [Column("role_id")]
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }

        [Column("mercado_pago_user_id")]
        public string? MercadoPagoUserId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public ICollection<VenueEntity> Venues { get; set; } = new List<VenueEntity>();
    }
}
