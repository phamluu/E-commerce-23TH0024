using E_commerce_23TH0024.Models.Order;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.Data
{
    public class OrderDbContext: DbContext
    {
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<ShippingRate> ShippingRates { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<DiscountRule> DiscountRules { get; set; }
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.ToTable("DonHang");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.DeliveryMethod)
                      .WithMany(p => p.DonHangs)
                      .HasForeignKey(e => e.IdDeliveryMethod);
                entity.HasOne(e => e.KhachHang);
                entity.HasMany(e => e.ChiTietDonHangs)
                      .WithOne(p => p.DonHang)
                      .HasForeignKey(e => e.IdDonHang);
                entity.HasMany(e => e.Payments).WithOne(p => p.DonHang).HasForeignKey(e => e.IdDonHang);

            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.DonHang)
                      .WithMany(p => p.Payments)
                      .HasForeignKey(e => e.IdDonHang);
            });
            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.ToTable("ChiTietDonHang");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.DonHang)
                      .WithMany(p => p.ChiTietDonHangs)
                      .HasForeignKey(e => e.IdDonHang);
                //entity.HasOne(e => e.SanPham)
                //      .WithMany(p => p.ChiTietDonHangs)
                //      .HasForeignKey(e => e.IdSanPham);

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

            });
            modelBuilder.Entity<DiscountRule>(entity =>
            {
                entity.ToTable("DiscountRules");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.CustomerType)
                      .WithMany(p => p.DiscountRules)
                      .HasForeignKey(e => e.IdCustomerType);
            });

        }


    }
}
