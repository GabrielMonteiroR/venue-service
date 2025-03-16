using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models
{
    [Table("equipament_brands")]
    public class EquipamentBrand
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("brand_name")]
        public string BrandName { get; set; }

        public ICollection<VenueEquipament> VenueEquipaments { get; set; }
    }
}
