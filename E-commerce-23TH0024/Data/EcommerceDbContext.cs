using E_commerce_23TH0024.Models.Ecommerce;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.Data
{
    public class EcommerceDbContext : DbContext
    {
        public DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductVariantAttribute> ProductVariantAttributes { get; set; }

        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<LoaiSanPham>(entity =>
            {
                entity.ToTable("LoaiSanPham");
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.SanPhams)
                        .WithOne(p => p.LoaiSanPham)
                        .HasForeignKey(e => e.IdLoaiSanPham);
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.ToTable("SanPham");
                entity.HasKey(e => e.Id);;
                entity.HasOne(e => e.LoaiSanPham)
                        .WithMany(p => p.SanPhams)
                        .HasForeignKey(e => e.IdLoaiSanPham);
                entity.HasMany(e => e.ProductVariants)
                        .WithOne(p => p.SanPham)
                        .HasForeignKey(e => e.IdSanPham);
               

            });

            modelBuilder.Entity<ProductAttribute>(entity =>
            {
                entity.ToTable("ProductAttributes");
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.AttributeValues)
                        .WithOne(p => p.ProductAttribute)
                        .HasForeignKey(e => e.IdProductAttribute);
                entity.HasMany(e => e.ProductVariantAttributes)
                        .WithOne(p => p.ProductAttribute)
                        .HasForeignKey(e => e.IdProductAttribute);
            });

            modelBuilder.Entity<AttributeValue>(entity =>
            {
                entity.ToTable("AttributeValues");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ProductAttribute)
                        .WithMany(p => p.AttributeValues)
                        .HasForeignKey(e => e.IdProductAttribute);
            });
            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.ToTable("ProductVariants");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.SanPham)
                        .WithMany(p => p.ProductVariants)
                        .HasForeignKey(e => e.IdSanPham);
                entity.HasMany(e => e.ProductVariantAttributes)
                        .WithOne(p => p.ProductVariant)
                        .HasForeignKey(e => e.IdProductVariant);
            });
            modelBuilder.Entity<ProductVariantAttribute>(entity =>
            {
                entity.ToTable("ProductVariantAttributes");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ProductVariant)
                        .WithMany(p => p.ProductVariantAttributes)
                        .HasForeignKey(e => e.IdProductVariant);
                entity.HasOne(e => e.ProductAttribute)
                        .WithMany(p => p.ProductVariantAttributes)
                        .HasForeignKey(e => e.IdProductAttribute);
                entity.HasOne(e => e.AttributeValue)
                        .WithMany(p => p.ProductVariantAttributes)
                        .HasForeignKey(e => e.IdAttributeValue);
            });
            
        }
    }
}
