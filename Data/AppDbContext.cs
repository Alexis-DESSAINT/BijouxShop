using Microsoft.EntityFrameworkCore;
using BijouxShop.Models;
namespace BijouxShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
        base(options) { }
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<Variant> Variants => Set<Variant>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);
            b.Entity<Category>().HasIndex(x => x.Slug).IsUnique();
            b.Entity<Product>().HasIndex(x => x.Slug).IsUnique();
            b.Entity<Cart>().HasIndex(x => x.SessionId).IsUnique();
            b.Entity<Variant>().HasIndex(x => x.Sku).IsUnique();
        }
    }
}
