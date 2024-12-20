using Microsoft.EntityFrameworkCore;
using TourBookingManagment.Model;

namespace TourBookingManagment.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .HasIndex(p => p.StripePaymentIntentId)
                .IsUnique();

            modelBuilder.Entity<Transaction>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}
