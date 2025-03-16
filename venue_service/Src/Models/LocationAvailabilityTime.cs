using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models
{
    [Table("location_availability_times")]
    public class LocationAvailabilityTime
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("status")]
        public VenueStatusEnum Status { get; set; }

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Required]
        [Column("available_time")]
        public DateTime AvailableTime { get; set; }

        [ForeignKey("VenueAvailabilityId")]
        [Column("venue_availability_id")]
        public int VenueAvailabilityId { get; set; }
        public VenueAvailability VenueAvailability { get; set; }
    }
}
