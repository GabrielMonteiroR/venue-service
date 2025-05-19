using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models.Venue
{
    [Table("venue_types")]
    public class VenueType
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
    }
}
