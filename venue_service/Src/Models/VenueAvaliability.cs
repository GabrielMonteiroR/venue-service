using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models
{
    [Table("venue_availability")]
    public class VenueAvailability
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("VenueId")]
        [Column("venue_id")]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        public ICollection<LocationAvailabilityTime> LocationAvailabilityTimes { get; set; }
    }
}
