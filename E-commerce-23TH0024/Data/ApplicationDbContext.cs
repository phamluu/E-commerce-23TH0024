using E_commerce_23TH0024.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<AspNetRole> AspNetRole { get; set; }
        public DbSet<AspNetUsers> AspNetUsers { get; set; }

        public DbSet<LoaiSanPham> LoaiSanPham { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        
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
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<NhomMenu> NhomMenus { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductVariantAttribute> ProductVariantAttributes { get; set; }
        public DbSet<ShippingRate> ShippingRates { get; set; }
        public DbSet<Ward> Wards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AspNetUsers>()
                .HasMany(u => u.AspNetUserLogins) 
                .WithOne(ul => ul.User)
                .HasForeignKey(ul => ul.UserId); 
            modelBuilder.Entity<AspNetUserLogin>()
                .HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });

            //modelBuilder.Entity<SanPham>(entity =>
            //{
            //    entity.ToTable("SanPham"); 
            //    entity.HasKey(e => e.Id); 
            //    entity.HasOne(d => d.LoaiSanPham)
            //          .WithMany()
            //          .HasForeignKey(d => d.MaLSP) 
            //          .HasConstraintName("FK_SanPham_LoaiSanPham");
            //});
        }
    }
}
