using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models.Venue
{
    [Table("venue_contact_infos")]
    public class VenueContactInfo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [MaxLength(20)]
        [Column("phone")]
        public string Phone { get; set; }

        [MaxLength(150)]
        [Column("email")]
        public string Email { get; set; }

        [ForeignKey("VenueId")]
        [Column("venue_id")]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }
    }
}
