using Microsoft.EntityFrameworkCore;
using Stripe;
using TourBookingManagment.Model;
namespace TourBookingManagment.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BookingDetails> BookingDetails { get; set; }
        public DbSet<TourBookingManagment.Model.Review> Reviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<TouristPlace> TouristPlaces { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserDetails)
                .WithOne(ud => ud.User)
                //.WithMany()
                .HasForeignKey<UserDetails>(ud => ud.UserId);


            modelBuilder.Entity<Transaction>()
                .HasIndex(p => p.StripePaymentIntentId)
                .IsUnique();

            modelBuilder.Entity<Transaction>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<TourBookingManagment.Model.Review>().ToTable("Reviews");

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });
        }
    }
}