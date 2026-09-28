using BusBookingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Route> Routes { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Route>().HasMany(r => r.Trips).WithOne(t => t.Route).HasForeignKey(t => t.RouteId);
            modelBuilder.Entity<Trip>().HasMany(t => t.Seats).WithOne(s => s.Trip).HasForeignKey(s => s.TripId);
            modelBuilder.Entity<Trip>().HasMany(t => t.Bookings).WithOne(b => b.Trip).HasForeignKey(b => b.TripId);

            modelBuilder.Entity<Seat>().HasIndex(s => new { s.TripId, s.SeatCode }).IsUnique();
            modelBuilder.Entity<Booking>().HasIndex(b => b.BookingCode).IsUnique();
        }
    }
}
