using EventEaseApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventEaseApp.Models
{
    public class EventEaseDbContext : DbContext
    {
        public EventEaseDbContext(DbContextOptions<EventEaseDbContext> options)
            : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent double booking - unique constraint
            modelBuilder.Entity<Booking>()
                .HasIndex(b => new { b.EventId, b.VenueId })
                .IsUnique()
                .HasDatabaseName("IX_Unique_EventVenue");

            // Configure relationships
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany(e => e.Bookings)  // Event has many Bookings
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Cascade);  // Delete Bookings when Event is deleted

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany(v => v.Bookings)  // Venue has many Bookings
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent venue deletion if bookings exist

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany()  // Venue doesn't need a collection of Events (only Bookings)
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.SetNull); // Set VenueId to null if venue is deleted

            // Optional: Configure table names
            modelBuilder.Entity<Venue>().ToTable("Venues");
            modelBuilder.Entity<Event>().ToTable("Events");
            modelBuilder.Entity<Booking>().ToTable("Bookings");
        }
    }
}