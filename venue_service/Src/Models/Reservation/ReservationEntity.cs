using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using venue_service.Src.Models.Payment;
using venue_service.Src.Models.User;
using venue_service.Src.Models.Venue;

[Table("reservations")]
public class ReservationEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public UserEntity User { get; set; }

    [Required]
    [Column("venue_id")]
    public int VenueId { get; set; }

    [ForeignKey("VenueId")]
    public VenueEntity Venue { get; set; }

    [Required]
    [Column("venue_availability_time_id")]
    public int VenueAvailabilityTimeId { get; set; }

    [ForeignKey("VenueAvailabilityTimeId")]
    public VenueAvailabilityTimeEntity VenueAvailabilityTime { get; set; }

    [Required]
    [Column("total_amount")]
    public decimal TotalAmount { get; set; }

    [Required]
    [Column("payment_method_id")]
    public int PaymentMethodId { get; set; }

    [ForeignKey("PaymentMethodId")]
    public PaymentMethodEntity PaymentMethod { get; set; }

    [InverseProperty(nameof(PaymentRecordEntity.Reservation))]
    public PaymentRecordEntity? PaymentRecord { get; set; }

    [Required]
    [Column("status")]
    public int Status { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Required]
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}
