using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models;

namespace venue_service.Src.Contexts;

public class DbContext
{
    public DbSet<Equipament> equipaments { get; set; }
    public DbSet<EquipamentType> equipamentTypes { get; set; }
    public DbSet<LocationAvailabilityTime> locationAvailabilityTimes { get; set; }
    public DbSet<Sport> sports { get; set; }
    public DbSet<User> users { get; set; }
    public DbSet<VenueContactInfo> venueContactInfos { get; set; }
    public DbSet<VenueImage> venueImages { get; set; }
    public DbSet<Venue> venues { get; set; }
    public DbSet<VenueStatusEnum> VenueStatusEnums { get; set; }
    public DbSet<VenueTypeEnum> venueTypeEnums { get; set; }
}
