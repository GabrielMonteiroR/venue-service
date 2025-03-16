using Microsoft.EntityFrameworkCore;

namespace venue_service.Src.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

    }
}
