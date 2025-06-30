using Microsoft.EntityFrameworkCore;
using MyEcommAPI.Models.Entities;

namespace backend.MyEcommAPI.Data
{
    public class MyEcommContext : DbContext
    {
        public MyEcommContext(DbContextOptions<MyEcommContext> options) : base(options)
        {     
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(ci => ci.CategoryId);

            modelBuilder.Entity<Order>()
                .HasOne(u => u.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(ui => ui.UserId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(p => p.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(p => p.Product)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(t => t.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(u => u.UnitPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}
