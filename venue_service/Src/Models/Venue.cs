using venue_service.Src.Models;
using venue_service.Src.Models.Venue;
using venue_service.Src.Models.Venue.Equipament;

namespace Src.Models;

public class Venue
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int Capacity { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Description { get; set; }
    public bool AllowLocalPayment { get; set; }
    public string Rules { get; set; }

    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public int VenueAvaliabilityId { get; set; }
    public VenueAvaliability VenueAvaliability { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<VenueImage> VenueImages { get; set; } 
    public ICollection<VenueEquipament> Equipaments { get; set; } 
    public ICollection<User_Venue> UserVenues { get; set; } 
}
