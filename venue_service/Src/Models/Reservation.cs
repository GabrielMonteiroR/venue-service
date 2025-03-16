using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models
{
    [Table("reservations")]
    public class Reservation
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("VenueId")]
        [Column("venue_id")]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        [ForeignKey("LocationAvailabilityTimeId")]
        [Column("location_availability_time_id")]
        public int LocationAvailabilityTimeId { get; set; }
        public LocationAvailabilityTime LocationAvailabilityTime { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("status")]
        public string Status { get; set; }

        [MaxLength(50)]
        [Column("payment_method")]
        public string PaymentMethod { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
