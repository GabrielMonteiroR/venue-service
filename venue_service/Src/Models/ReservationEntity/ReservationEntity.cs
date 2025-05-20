using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using venue_service.Src.Models.Payment;

namespace venue_service.Src.Models.Reservation
{
    [Table("reservations")]
    public class ReservationEntity
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


        [Required]
        [MaxLength(50)]
        [Column("status")]
        public int Status { get; set; }

        [Required]
        [Column("venue_availability_time_id")]
        public int VenueAvailabilityTimeId { get; set; }

        [ForeignKey("PaymentMethodId")]
        [Column("payment_method_id")]
        public int PaymentMethodId { get; set; }
        public PaymentMethodEntity PaymentMethod { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } 

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } 

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
    }
}