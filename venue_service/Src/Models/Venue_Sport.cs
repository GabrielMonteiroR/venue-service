using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models
{
    [Table("venue_sports")]
    public class Venue_Sport
    {
        [ForeignKey("VenueId")]
        [Column("venue_id")]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        [ForeignKey("SportId")]
        [Column("sport_id")]
        public int SportId { get; set; }
        public Sport Sport { get; set; }
    }
}
