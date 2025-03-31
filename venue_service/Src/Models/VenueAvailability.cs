using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models
{
    public class VenueAvailability
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("start_date")]
        public String StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public String EndDate { get; set; }

        [Required]
        [Column("start_time")]
        public double price { get; set; }
    }
}
