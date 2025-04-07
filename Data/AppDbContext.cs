using Microsoft.EntityFrameworkCore;
using MormorBageriAPI.Models;

namespace MormorBageriAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<RawMaterialSupplier> RawMaterialSuppliers { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderRow> OrderRows { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RawMaterialSupplier>()
                .HasKey(rs => new { rs.RawMaterialId, rs.SupplierId });

            modelBuilder.Entity<RawMaterialSupplier>()
                .HasOne(rs => rs.RawMaterial)
                .WithMany(r => r.RawMaterialSuppliers)
                .HasForeignKey(rs => rs.RawMaterialId);

            modelBuilder.Entity<RawMaterialSupplier>()
                .HasOne(rs => rs.Supplier)
                .WithMany(s => s.RawMaterialSuppliers)
                .HasForeignKey(rs => rs.SupplierId);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Address)
                .WithOne(a => a.Customer)
                .HasForeignKey<Address>(a => a.CustomerId);

            modelBuilder.Entity<OrderRow>()
                .HasOne(or => or.Product)
                .WithMany(p => p.OrderRows)
                .HasForeignKey(or => or.ProductId);

            modelBuilder.Entity<OrderRow>()
                .HasOne(or => or.Order)
                .WithMany(o => o.OrderRows)
                .HasForeignKey(or => or.OrderId);
        }
    }
}
