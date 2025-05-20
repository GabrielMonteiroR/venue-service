using System.ComponentModel.DataAnnotations.Schema;
using venue_service.Src.Models.Sport;

namespace venue_service.Src.Models.Venue
{
    [Table("venue_sports")]
    public class Venue_SportEntity
    {
        [ForeignKey("VenueId")]
        [Column("venue_id")]
        public int VenueId { get; set; }
        public VenueEntity Venue { get; set; }

        [ForeignKey("SportId")]
        [Column("sport_id")]
        public int SportId { get; set; }
        public SportEntity Sport { get; set; }
    }
}
