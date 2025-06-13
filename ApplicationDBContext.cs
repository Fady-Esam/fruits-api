

using FruitsAppBackEnd.BL;
using FruitsAppBackEnd.Domain;
using Humanizer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FruitsAppBackEnd
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Product>()
                .Property(p => p.Quantity)
            .HasDefaultValue(1);
            modelBuilder.Entity<Product>()
                .Property(p => p.Discount)
                .HasDefaultValue(0);
            modelBuilder.Entity<Product>()
                .Property(p => p.PriceUnit)
                .HasDefaultValue("Kilo");
            modelBuilder.Entity<Product>()
                .Property(p => p.SellingCount)
                .HasDefaultValue(0);
            modelBuilder.Entity<Product>()
            .Property(p => p.CategoryId)
            .HasDefaultValue(2);
            modelBuilder.Entity<Order>()
            .Property(p => p.Data)
            .HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Review>()
            .Property(p => p.Date)
            .HasDefaultValue(DateTime.Now);

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rate> Rates { get; set; }
    }
}
