using Piloto.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
namespace Piloto.Api.Infrastructure.Data
{
    public class StockManagementDBContext : DbContext
        //, IDbContextFactory<MySQLStockManagementDBContext>
    {
        //private readonly DbContextOptions _options;

        //public MySQLStockManagementDBContext(DbContextOptions options) : base(options)
        //{
        //    Console.WriteLine("options", options);
        //    _options = options;
        //}
        public StockManagementDBContext(DbContextOptions<StockManagementDBContext> options) : base(options)
        {
            Console.WriteLine("options", options);
        }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductSupplier> ProductSuppliers => Set<ProductSupplier>("ProductSupplier");

        public DbSet<SupplierAddress> SupplierAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>(ConfigureProduct);
            builder.Entity<Supplier>(ConfigureSupplier);
            builder.Entity<ProductSupplier>(ConfigureProductSupplier);
            builder.Entity<SupplierAddress>( ConfigureSupplierAddress);

        }
        private void ConfigureSupplierAddress(EntityTypeBuilder<SupplierAddress> builder)
        {
            builder.ToTable("SupplierAddress");
            builder.HasKey(sa => sa.Id);
            builder.Property(sa => sa.Id).ValueGeneratedOnAdd();

            builder.Ignore(sa => sa.ValidationResult);

            builder.Property(sa => sa.Name)
              .IsRequired()
              .HasMaxLength(150);

            builder.Property(sa => sa.Country)
              .IsRequired()
              .HasMaxLength(50);
       
            builder.Property(sa => sa.Street)
              .IsRequired()
              .HasMaxLength(150);
            builder.Property(sa => sa.City)
              .IsRequired()
              .HasMaxLength(50);

            builder.Property(sa => sa.PostalCode)
              .IsRequired()
              .HasMaxLength(50);

            builder.Property(sa => sa.HouseNumber)
              .IsRequired()
              .HasMaxLength(20);

            builder.HasOne(sa => sa.Supplier)
              .WithMany(s => s.SupplierAddresses).
              HasForeignKey(sa => sa.SupplierId);


        }

        private void ConfigureProduct(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Price)
              .IsRequired();
            builder.Property(p => p.Stock)
             .IsRequired();

            builder.Ignore(p => p.ValidationResult);
              
        }
        private void ConfigureProductSupplier(EntityTypeBuilder<ProductSupplier> builder)
        {


            builder.ToTable("ProductSupplier");
            builder.HasKey("Id");  
            builder.HasOne(ps =>ps.Product)
                .WithMany(p=> p.ProductSuppliers)
                .HasForeignKey(ps => ps.ProductId);
            builder.HasOne(ps => ps.Supplier)
                .WithMany(s=>s.ProductSuppliers)
                .HasForeignKey(ps => ps.SupplierId);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasIndex(new[] { "ProductId" }, "IX_ProductSupplier_ProductId");
            builder.Ignore(ps => ps.ValidationResult);


            builder.Ignore(p => p.ValidationResult);

        }

        private void ConfigureSupplier(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Supplier");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd();

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.CNPJ)
                .IsRequired()
                .HasMaxLength(20);
            builder.Ignore(s => s.ValidationResult);
           

            builder.HasMany(s => s.SupplierAddresses)
                .WithOne(sa => sa.Supplier)
                .HasForeignKey(sa => sa.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

        }

       


        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DateCreated") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DateCreated").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DateCreated").IsModified = false;
                }


            }

            return base.SaveChanges();
        }

        //public MySQLStockManagementDBContext CreateDbContext()
        //{
        //    return new MySQLStockManagementDBContext(_options);
        //}
    }
}
