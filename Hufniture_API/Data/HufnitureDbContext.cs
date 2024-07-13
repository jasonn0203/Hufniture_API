using Hufniture_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hufniture_API.Data
{
    public class HufnitureDbContext : IdentityDbContext<AppUser>
    {
        public HufnitureDbContext(DbContextOptions options) : base(options)
        {
        }

        // Models
        public DbSet<FurnitureProduct> FurnitureProducts { get; set; }
        public DbSet<FurnitureCategory> FurnitureCategories { get; set; }
        public DbSet<FurnitureType> FurnitureTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            //Config relationships of the database ( 1-1, Many-Many, 1-Many )

            builder.Entity<FurnitureProduct>()
               .HasOne(p => p.FurnitureCategory)
               .WithMany(c => c.FurnitureProducts)
               .HasForeignKey(p => p.FurnitureCategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FurnitureProduct>()
                .HasOne(p => p.FurnitureType)
                .WithMany(t => t.FurnitureProducts)
                .HasForeignKey(p => p.FurnitureTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FurnitureType>()
                .HasOne(t => t.FurnitureCategory)
                .WithMany(c => c.FurnitureTypes)
                .HasForeignKey(t => t.FurnitureCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FurnitureProduct>()
                .HasOne(p => p.Color)
                .WithMany(c => c.FurnitureProducts)
                .HasForeignKey(p => p.ColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FurnitureProduct>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.FurnitureProduct)
                .WithMany()
                .HasForeignKey(oi => oi.FurnitureProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.FurnitureProduct)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.FurnitureProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderStatus>()
                .HasOne(os => os.Order)
                .WithMany(o => o.OrderStatuses)
                .HasForeignKey(os => os.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
