using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models
{
    [Table("equipament_types")]
    public class EquipamentType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("type_name")]
        public string TypeName { get; set; }

        public ICollection<VenueEquipament> VenueEquipaments { get; set; }
    }
}
