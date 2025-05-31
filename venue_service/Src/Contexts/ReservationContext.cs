using Microsoft.EntityFrameworkCore;
using venue_service.Src.Models.Payment;
using venue_service.Src.Models.Venue;

namespace venue_service.Src.Contexts
{
    public class ReservationContext : DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options) : base(options) { }

        public DbSet<ReservationEntity> Reservations => Set<ReservationEntity>();
        public DbSet<PaymentMethodEntity> PaymentMethods => Set<PaymentMethodEntity>();
        public DbSet<PaymentRecordEntity> PaymentRecords => Set<PaymentRecordEntity>();
        public DbSet<SchedulesEntity> Schedules => Set<SchedulesEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<Venue_SportEntity>();

            modelBuilder.Entity<ReservationEntity>().ToTable("reservations");
            modelBuilder.Entity<PaymentMethodEntity>().ToTable("payment_methods");
            modelBuilder.Entity<PaymentRecordEntity>().ToTable("payment_records");
            modelBuilder.Entity<SchedulesEntity>().ToTable("schedules");

            // Reservation -> User
            modelBuilder.Entity<ReservationEntity>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            // Reservation -> Venue
            modelBuilder.Entity<ReservationEntity>()
                .HasOne(r => r.Venue)
                .WithMany()
                .HasForeignKey(r => r.VenueId);

            // Reservation -> PaymentMethod
            modelBuilder.Entity<ReservationEntity>()
                .HasOne(r => r.PaymentMethod)
                .WithMany(pm => pm.Reservations)
                .HasForeignKey(r => r.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reservation -> Schedule (1:1)
            modelBuilder.Entity<ReservationEntity>()
                .HasOne(r => r.Schedule)
                .WithOne(s => s.Reservation)
                .HasForeignKey<ReservationEntity>(r => r.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            // PaymentRecord -> Reservation (1:1)
            modelBuilder.Entity<PaymentRecordEntity>()
                .HasOne(pr => pr.Reservation)
                .WithOne(r => r.PaymentRecord)
                .HasForeignKey<PaymentRecordEntity>(pr => pr.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Schedule -> Venue (muitos horários por venue)
            modelBuilder.Entity<SchedulesEntity>()
                .HasOne(s => s.Venue)
                .WithMany(v => v.Schedules)
                .HasForeignKey(s => s.VenueId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
