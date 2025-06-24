using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using venue_service.Src.Models.User;

namespace venue_service.Src.Models.Venue;

[Table("venues")]
public class VenueEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("street")]
    public string Street { get; set; }

    [MaxLength(10)]
    [Column("number")]
    public string? Number { get; set; }

    [MaxLength(100)]
    [Column("complement")]
    public string? Complement { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("neighborhood")]
    public string Neighborhood { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("city")]
    public string City { get; set; }

    [Required]
    [MaxLength(2)]
    [Column("state")]
    public string State { get; set; } 

    [Required]
    [MaxLength(9)]
    [Column("postal_code")]
    public string PostalCode { get; set; } 

    [Column("latitude")]
    public double Latitude { get; set; }

    [Column("longitude")]
    public double Longitude { get; set; }

    [MaxLength(500)]
    [Column("description")]
    public string Description { get; set; }

    [ForeignKey("VenueTypeId")]
    [Column("venue_type_id")]
    public int VenueTypeId { get; set; }
    public VenueTypeEntity VenueType { get; set; }

    [MaxLength(500)]
    [Column("rules")]
    public string Rules { get; set; }

    [Required]
    [Column("capacity")]
    public int Capacity { get; set; }

    [ForeignKey("OwnerId")]
    [Column("owner_id")]
    public int OwnerId { get; set; }
    public UserEntity Owner { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public ICollection<VenueImageEntity> VenueImages { get; set; }
    public ICollection<VenueEquipamentEntity> VenueEquipaments { get; set; }
    public ICollection<Venue_SportEntity> VenueSports { get; set; }
    public ICollection<VenueContactInfoEntity> VenueContactInfos { get; set; }
    public ICollection<VenueAvailabilityTimeEntity>? VenueAvailabilityTimes { get; set; }
}

