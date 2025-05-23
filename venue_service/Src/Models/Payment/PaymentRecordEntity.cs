using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models.Payment
{
    [Table("payment_records")]
    public class PaymentRecordEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("reservation_id")]
        public int ReservationId { get; set; }

        [ForeignKey("ReservationId")]
        [InverseProperty(nameof(ReservationEntity.PaymentRecord))]
        public ReservationEntity Reservation { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; } 

        [Column("mercado_pago_payment_id")]
        public string MercadoPagoPaymentId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("paid_at")]
        public DateTime? PaidAt { get; set; }
    }
}
