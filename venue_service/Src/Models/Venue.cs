using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace venue_service.Src.Models
{
    [Table("venues")]
    public class Venue
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        [Column("address")]
        public string Address { get; set; }

        [Column("capacity")]
        public int Capacity { get; set; }

        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("longitude")]
        public double Longitude { get; set; }

        [MaxLength(500)]
        [Column("description")]
        public string Description { get; set; }

        [Column("allow_local_payment")]
        public bool AllowLocalPayment { get; set; }

        [ForeignKey("VenueTypeId")]
        [Column("venue_type_id")]
        public int VenueTypeId { get; set; }
        public VenueType VenueType { get; set; }


        [MaxLength(500)]
        [Column("rules")]
        public string Rules { get; set; }

        [ForeignKey("OwnerId")]
        [Column("owner_id")]
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        public ICollection<VenueImage> VenueImages { get; set; }
        public ICollection<VenueEquipament> VenueEquipaments { get; set; }
        public ICollection<User_Venue> UserVenues { get; set; }
        public ICollection<Venue_Sport> VenueSports { get; set; }
        public ICollection<VenueContactInfo> VenueContactInfos { get; set; }
        public ICollection<VenueAvailabilityTime>? VenueAvailabilityTimes { get; set; }
    }
}
