using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models
{
    [Table("venue_availability_times")]
    public class VenueAvailabilityTime
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [ForeignKey("VenueId")]
        [Column("venue_id")]
        public int VenueId { get; set; }

        [Required]
        [Column("price", TypeName = "numeric")]
        public decimal Price { get; set; }


        [Required]
        [Column("time_status")]
        [MaxLength(50)]
        public string TimeStatus { get; set; }

        [Required]
        [Column("is_reserved")]
        public bool IsReserved { get; set; }
    }
}
