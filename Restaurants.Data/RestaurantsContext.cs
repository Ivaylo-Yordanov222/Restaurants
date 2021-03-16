using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurants.Models;

namespace Restaurants.Data
{
    public class RestaurantsContext : IdentityDbContext<User>
    {
        public RestaurantsContext(DbContextOptions<RestaurantsContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductsOrders> ProductsOrders { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Preference> Preferences { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
               .Entity<Order>()
               .HasMany(o => o.ProductsInOrder)
               .WithOne(po => po.Order)
               .HasForeignKey(po => po.OrderId);

            builder
                .Entity<Product>()
                .HasMany(p => p.OrdersWithProduct)
                .WithOne(op => op.Product)
                .HasForeignKey(c => c.ProductId);

            builder
                .Entity<ProductsOrders>()
                .HasKey(po => new { po.OrderId, po.ProductId });

            builder
                .Entity<Category>()
                .HasIndex(c => c.Name);

            builder
                .Entity<Product>()
                .HasIndex(p => p.Name);

            base.OnModelCreating(builder);
        }

    }
}
