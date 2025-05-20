using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace venue_service.Src.Models.Venue
{
    [Table("venue_equipaments")]
    public class VenueEquipamentEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [ForeignKey("VenueId")]
        [Column("venue_id")]
        public int VenueId { get; set; }
        public VenueEntity Venue { get; set; }
    }
}
