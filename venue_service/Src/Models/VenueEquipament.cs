using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace venue_service.Src.Models
{
    [Table("venue_equipaments")]
    public class VenueEquipament
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("equipament_name")]
        public string EquipamentName { get; set; }

        [ForeignKey("EquipamentBrandId")]
        [Column("equipament_brand_id")]
        public int EquipamentBrandId { get; set; }
        public EquipamentBrand EquipamentBrand { get; set; }

        [ForeignKey("EquipamentTypeId")]
        [Column("equipament_type_id")]
        public int EquipamentTypeId { get; set; }
        public EquipamentType EquipamentType { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [ForeignKey("VenueId")]
        [Column("venue_id")]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }
    }
}
