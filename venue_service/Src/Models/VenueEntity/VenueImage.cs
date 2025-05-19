using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models.Venue
{
    [Table("venue_images")]
    public class VenueImage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("file_name")]
        public string FileName { get; set; }

        [Required]
        [MaxLength(300)]
        [Column("image_url")]
        public string ImageUrl { get; set; }

        [ForeignKey("VenueId")]
        [Column("venue_id")]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }
    }
}
