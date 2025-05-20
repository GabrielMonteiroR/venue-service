using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models.Payment
{
    [Table("payment_methods")]
    public class PaymentMethodEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; }

        [MaxLength(200)]
        [Column("description")]
        public string Description { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
