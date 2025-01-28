using Microsoft.EntityFrameworkCore;
using TourBookingManagment.Model;

namespace TourBookingManagment.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BookingDetails> BookingDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User and UserDetails relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserDetails)
                .WithOne(ud => ud.User)
                .HasForeignKey<UserDetails>(ud => ud.UserId);

            // Existing configurations
            modelBuilder.Entity<Transaction>()
                .HasIndex(p => p.StripePaymentIntentId)
                .IsUnique();

            modelBuilder.Entity<Transaction>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}
