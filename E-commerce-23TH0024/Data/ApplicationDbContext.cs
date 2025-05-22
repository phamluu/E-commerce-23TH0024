using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Models.Identity;
using E_commerce_23TH0024.Models.Location;
using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Models.SystemSetting;
using E_commerce_23TH0024.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_commerce_23TH0024.Models;

namespace E_commerce_23TH0024.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        //public DbSet<AspNetRole> AspNetRole { get; set; }
        //public DbSet<AspNetUsers> AspNetUsers { get; set; }

        public DbSet<LoaiSanPham> LoaiSanPham { get; set; }
        public DbSet<SanPham> SanPham { get; set; }
        
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<DiscountRule> DiscountRules { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        //public DbSet<ErrorViewModel> ErrorViewModel { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<NhomMenu> NhomMenus { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductVariantAttribute> ProductVariantAttributes { get; set; }
        public DbSet<ShippingRate> ShippingRates { get; set; }
        public DbSet<Ward> Wards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.ToTable("NhanVien");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.AspNetUser).WithOne(u => u.NhanVien)
                .HasForeignKey<NhanVien>(e => e.IdAspNetUsers);
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.ToTable("DonHang");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.DeliveryMethod)
                      .WithMany(p => p.DonHangs)
                      .HasForeignKey(e => e.IdDeliveryMethod);
                entity.HasOne(e => e.KhachHang)
                      .WithMany(p => p.DonHangs)
                      .HasForeignKey(e => e.IdKhachHang);
                entity.HasMany(e => e.ChiTietDonHangs)
                      .WithOne(p => p.DonHang)
                      .HasForeignKey(e => e.IdDonHang);

            });

            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.ToTable("ChiTietDonHang");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.DonHang)
                      .WithMany(p => p.ChiTietDonHangs)
                      .HasForeignKey(e => e.IdDonHang);
                entity.HasOne(e => e.SanPham)
                      .WithMany(p => p.ChiTietDonHangs)
                      .HasForeignKey(e => e.IdSanPham);

            });
            modelBuilder.Entity<DeliveryMethod>(entity =>
            {
                entity.ToTable("DeliveryMethods");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MethodName).IsRequired().HasMaxLength(100);
                entity.HasMany(e => e.DonHangs)
                      .WithOne(p => p.DeliveryMethod)
                      .HasForeignKey(e => e.IdDeliveryMethod);
                entity.HasMany(e => e.ShippingRates)
                      .WithOne(p => p.DeliveryMethod)
                      .HasForeignKey(e => e.IdDeliveryMethod);

            });
            modelBuilder.Entity<ShippingRate>(entity =>
            {
                entity.ToTable("ShippingRates");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.DeliveryMethod)
                      .WithMany(p => p.ShippingRates)
                      .HasForeignKey(e => e.IdDeliveryMethod);
            });
            modelBuilder.Entity<CustomerType>(entity =>
            {
                entity.ToTable("CustomerTypes");
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.KhachHangs)
                      .WithOne(p => p.CustomerType)
                      .HasForeignKey(e => e.IdCustomerType);
                entity.HasMany(e => e.DiscountRules)
                      .WithOne(p => p.CustomerType)
                      .HasForeignKey(e => e.IdCustomerType);

            });
            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.ToTable("KhachHang");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.CustomerType)
                      .WithMany(p => p.KhachHangs)
                      .HasForeignKey(e => e.IdCustomerType);
                entity.HasMany(e => e.DonHangs)
                      .WithOne(p => p.KhachHang)
                      .HasForeignKey(e => e.IdKhachHang);

            });
            modelBuilder.Entity<DiscountRule>(entity =>
            {
                entity.ToTable("DiscountRules");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.LoaiSanPham)
                      .WithMany(p => p.DiscountRules)
                      .HasForeignKey(e => e.IdLoaiSanPham);
                entity.HasOne(e => e.CustomerType)
                      .WithMany(p => p.DiscountRules)
                      .HasForeignKey(e => e.IdCustomerType);
            });
            {
                modelBuilder.Entity<LoaiSanPham>(entity =>
                {
                    entity.ToTable("LoaiSanPham");
                    entity.HasKey(e => e.Id);
                    entity.HasMany(e => e.SanPhams)
                          .WithOne(p => p.LoaiSanPham)
                          .HasForeignKey(e => e.IdLoaiSanPham);
                    entity.HasMany(e => e.DiscountRules)
                          .WithOne(p => p.LoaiSanPham)
                          .HasForeignKey(e => e.IdLoaiSanPham);
                });

                modelBuilder.Entity<SanPham>(entity =>
                {
                    entity.ToTable("SanPham");
                    entity.HasKey(e => e.Id); ;
                    entity.HasOne(e => e.LoaiSanPham)
                          .WithMany(p => p.SanPhams)
                          .HasForeignKey(e => e.IdLoaiSanPham);
                    entity.HasMany(e => e.ProductVariants)
                          .WithOne(p => p.SanPham)
                          .HasForeignKey(e => e.IdSanPham);
                    entity.HasMany(e => e.ChiTietDonHangs)
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
        //public DbSet<E_commerce_23TH0024.Models.LoaiSanPhamViewModels> LoaiSanPhamViewModels { get; set; } = default!;
    }
}
