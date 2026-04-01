using Microsoft.EntityFrameworkCore;
using Products.Api.Models;

namespace Products.Api.Data;

public sealed class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(2000).IsRequired();
            
            // Configure complex types as owned entities
            entity.OwnsOne(e => e.Details, details =>
            {
                details.Property(d => d.Brand).HasMaxLength(100).IsRequired();
                details.Property(d => d.Category).HasMaxLength(100).IsRequired();
                details.Property(d => d.SubCategory).HasMaxLength(100).IsRequired();
                details.Property(d => d.Manufacturer).HasMaxLength(100).IsRequired();
                details.Property(d => d.CountryOfOrigin).HasMaxLength(100).IsRequired();
                details.Property(d => d.Tags).HasColumnType("json");
            });

            entity.OwnsOne(e => e.Pricing);
            entity.OwnsOne(e => e.Inventory);
            
            entity.OwnsOne(e => e.Specifications, specs =>
            {
                specs.Property(s => s.Dimensions).HasColumnType("json");
                specs.Property(s => s.Materials).HasColumnType("json");
                specs.Property(s => s.TechnicalSpecs).HasColumnType("json");
            });
        });
    }
} 