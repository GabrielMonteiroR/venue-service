using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models.Venue
{
    [Table("venue_availability_times")]
    public class VenueAvailabilityTimeEntity
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
    [Column("venue_id")]
    public int VenueId { get; set; }

    [ForeignKey("VenueId")]
        public VenueEntity Venue { get; set; }

        [Required]
        [Column("price", TypeName = "numeric")]
        public decimal Price { get; set; }

        [Required]
        [Column("is_reserved")]
        public bool IsReserved { get; set; }

        [Column("reserved_by")]
        public int? UserId { get; set; }

    }
}
