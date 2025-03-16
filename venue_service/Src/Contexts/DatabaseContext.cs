using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models;

namespace venue_service.Src.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User_Venue>()
                .HasKey(uv => new { uv.UserId, uv.VenueId });

            modelBuilder.Entity<Venue_Sport>()
                .HasKey(vs => new { vs.VenueId, vs.SportId });

        }
    }




}
}
