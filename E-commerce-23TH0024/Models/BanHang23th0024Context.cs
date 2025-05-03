//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace E_commerce_23TH0024.Models;

//public partial class BanHang23th0024Context : DbContext
//{
//    public BanHang23th0024Context()
//    {
//    }

//    public BanHang23th0024Context(DbContextOptions<BanHang23th0024Context> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

//    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

//    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

//    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

//    public virtual DbSet<AttributeValue> AttributeValues { get; set; }

//    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

//    public virtual DbSet<City> Cities { get; set; }

//    public virtual DbSet<Configuration> Configurations { get; set; }

//    public virtual DbSet<CustomerType> CustomerTypes { get; set; }

//    public virtual DbSet<DeliveryMethod> DeliveryMethods { get; set; }

//    public virtual DbSet<DiscountRule> DiscountRules { get; set; }

//    public virtual DbSet<District> Districts { get; set; }

//    public virtual DbSet<DonHang> DonHangs { get; set; }

//    public virtual DbSet<KhachHang> KhachHangs { get; set; }

//    public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }

//    public virtual DbSet<Menu> Menus { get; set; }

//    public virtual DbSet<NhanVien> NhanViens { get; set; }

//    public virtual DbSet<NhomMenu> NhomMenus { get; set; }

//    public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }

//    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

//    public virtual DbSet<ProductVariantAttribute> ProductVariantAttributes { get; set; }

//    public virtual DbSet<SanPham> SanPhams { get; set; }

//    public virtual DbSet<ShippingRate> ShippingRates { get; set; }

//    public virtual DbSet<Ward> Wards { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=ADMIN\\SQLEXPRESS;Database=E_commerce_23TH0024;Trusted_Connection=True;Encrypt=False;");

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<AspNetRole>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetRoles");

//            entity.Property(e => e.Id).HasMaxLength(128);
//            entity.Property(e => e.Name).HasMaxLength(256);
//        });

//        modelBuilder.Entity<AspNetUser>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetUsers");

//            entity.Property(e => e.Id).HasMaxLength(128);
//            entity.Property(e => e.Email).HasMaxLength(256);
//            entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");
//            entity.Property(e => e.UserName).HasMaxLength(256);

//            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
//                .UsingEntity<Dictionary<string, object>>(
//                    "AspNetUserRole",
//                    r => r.HasOne<AspNetRole>().WithMany()
//                        .HasForeignKey("RoleId")
//                        .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId"),
//                    l => l.HasOne<AspNetUser>().WithMany()
//                        .HasForeignKey("UserId")
//                        .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId"),
//                    j =>
//                    {
//                        j.HasKey("UserId", "RoleId").HasName("PK_dbo.AspNetUserRoles");
//                        j.ToTable("AspNetUserRoles");
//                        j.IndexerProperty<string>("UserId").HasMaxLength(128);
//                        j.IndexerProperty<string>("RoleId").HasMaxLength(128);
//                    });
//        });

//        modelBuilder.Entity<AspNetUserClaim>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetUserClaims");

//            entity.Property(e => e.UserId).HasMaxLength(128);

//            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims)
//                .HasForeignKey(d => d.UserId)
//                .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
//        });

//        modelBuilder.Entity<AspNetUserLogin>(entity =>
//        {
//            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId }).HasName("PK_dbo.AspNetUserLogins");

//            entity.Property(e => e.LoginProvider).HasMaxLength(128);
//            entity.Property(e => e.ProviderKey).HasMaxLength(128);
//            entity.Property(e => e.UserId).HasMaxLength(128);

//            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins)
//                .HasForeignKey(d => d.UserId)
//                .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
//        });

//        modelBuilder.Entity<AttributeValue>(entity =>
//        {
//            entity.HasKey(e => e.ValueId).HasName("PK_VariantOption");

//            entity.ToTable("AttributeValue");

//            entity.Property(e => e.ValueId).HasColumnName("ValueID");
//            entity.Property(e => e.AttributeId)
//                .HasMaxLength(20)
//                .IsUnicode(false)
//                .HasColumnName("AttributeID");
//            entity.Property(e => e.Value).HasMaxLength(50);

//            entity.HasOne(d => d.Attribute).WithMany(p => p.AttributeValues)
//                .HasForeignKey(d => d.AttributeId)
//                .OnDelete(DeleteBehavior.Cascade)
//                .HasConstraintName("FK_AttributeValue_Attribute");
//        });

//        modelBuilder.Entity<ChiTietDonHang>(entity =>
//        {
//            entity.HasKey(e => e.Ma);

//            entity.ToTable("ChiTietDonHang");

//            entity.Property(e => e.DiscountApplied).HasColumnType("decimal(18, 0)");
//            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 0)");
//            entity.Property(e => e.MaSp).HasColumnName("MaSP");
//            entity.Property(e => e.SoHd).HasColumnName("SoHD");

//            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietDonHangs)
//                .HasForeignKey(d => d.MaSp)
//                .HasConstraintName("FK_ChiTietDonHang_SanPham");

//            entity.HasOne(d => d.SoHdNavigation).WithMany(p => p.ChiTietDonHangs)
//                .HasForeignKey(d => d.SoHd)
//                .HasConstraintName("FK_ChiTietDonHang_DonHang");
//        });

//        modelBuilder.Entity<City>(entity =>
//        {
//            entity.Property(e => e.CityIdD).HasColumnName("CityID");
//            entity.Property(e => e.CityName).HasMaxLength(100);
//        });

//        modelBuilder.Entity<Configuration>(entity =>
//        {
//            entity.HasKey(e => e.ConfigId).HasName("PK_Configuration_1");

//            entity.ToTable("Configuration");

//            entity.HasIndex(e => e.ConfigId, "IX_Configuration").IsUnique();

//            entity.HasIndex(e => e.ConfigId, "IX_Configuration_1");

//            entity.Property(e => e.ConfigCode).HasMaxLength(50);
//            entity.Property(e => e.ConfigName).HasMaxLength(100);
//            entity.Property(e => e.ConfigValue).HasColumnType("ntext");
//        });

//        modelBuilder.Entity<CustomerType>(entity =>
//        {
//            entity.HasKey(e => e.CustomerTypeId).HasName("PK_CustomType");

//            entity.ToTable("CustomerType");

//            entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");
//            entity.Property(e => e.CustomerTypeName).HasMaxLength(50);
//        });

//        modelBuilder.Entity<DeliveryMethod>(entity =>
//        {
//            entity.HasKey(e => e.ShippingMethodID).HasName("PK_ShippingMethod");

//            entity.ToTable("DeliveryMethod");

//            entity.Property(e => e.ShippingMethodID).HasColumnName("ShippingMethodID");
//            entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");
//            entity.Property(e => e.DeliveryTime).HasMaxLength(150);
//            entity.Property(e => e.Description).HasMaxLength(200);
//            entity.Property(e => e.MethodName).HasMaxLength(100);
//        });

//        modelBuilder.Entity<DiscountRule>(entity =>
//        {
//            entity.HasKey(e => e.RuleId);

//            entity.Property(e => e.RuleId).HasColumnName("RuleID");
//            entity.Property(e => e.Created_at)
//                .HasColumnType("datetime")
//                .HasColumnName("Created_at");
//            entity.Property(e => e.CustomerTypeID).HasColumnName("CustomerTypeID");
//            entity.Property(e => e.Description).HasMaxLength(255);
//            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 0)");
//            entity.Property(e => e.DiscountType)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasColumnName("Discount_Type");
//            entity.Property(e => e.EndDate).HasColumnType("datetime");
//            entity.Property(e => e.MinTotalPrice).HasColumnType("decimal(18, 0)");
//            entity.Property(e => e.Name).HasMaxLength(50);
//            entity.Property(e => e.ProductGroupID).HasColumnName("ProductGroupID");
//            entity.Property(e => e.StartDate).HasColumnType("datetime");

//            entity.HasOne(d => d.CustomerType).WithMany(p => p.DiscountRules)
//                .HasForeignKey(d => d.CustomerTypeID)
//                .HasConstraintName("FK_DiscountRules_CustomerType");

//            entity.HasOne(d => d.ProductGroupID).WithMany(p => p.DiscountRules)
//                .HasForeignKey(d => d.ProductGroupId)
//                .HasConstraintName("FK_DiscountRules_LoaiSanPham");
//        });

//        modelBuilder.Entity<District>(entity =>
//        {
//            entity.Property(e => e.DistrictID).HasColumnName("DistrictID");
//            entity.Property(e => e.CityID).HasColumnName("CityID");
//            entity.Property(e => e.DistrictName).HasMaxLength(50);

//            entity.HasOne(d => d.City).WithMany(p => p.Districts)
//                .HasForeignKey(d => d.CityID)
//                .HasConstraintName("FK_Districts_Cities");
//        });

//        modelBuilder.Entity<DonHang>(entity =>
//        {
//            entity.HasKey(e => e.SoHD).HasName("PK__DonHang__BC3CAB57BFB24B72");

//            entity.ToTable("DonHang");

//            entity.Property(e => e.SoHD).HasColumnName("SoHD");
//            entity.Property(e => e.DiscountRuleId).HasColumnName("DiscountRuleID");
//            entity.Property(e => e.MaKH).HasColumnName("MaKH");
//            entity.Property(e => e.MaNVDuyet).HasColumnName("MaNVDuyet");
//            entity.Property(e => e.MaNVGH).HasColumnName("MaNVGH");
//            entity.Property(e => e.NgayDatHang).HasColumnType("datetime");
//            entity.Property(e => e.NgayGiaoHang).HasColumnType("datetime");
//            entity.Property(e => e.ShippingFee).HasColumnType("decimal(18, 0)");
//            entity.Property(e => e.ShippingMethodID).HasColumnName("ShippingMethodID");
//            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");
//            entity.Property(e => e.TotalProductAmount).HasColumnType("decimal(18, 0)");
//            entity.Property(e => e.VAT)
//                .HasColumnType("decimal(18, 0)")
//                .HasColumnName("VAT");

//            entity.HasOne(d => d.DiscountRule).WithMany(p => p.DonHangs)
//                .HasForeignKey(d => d.DiscountRuleId)
//                .HasConstraintName("FK_DonHang_DiscountRules");

//            entity.HasOne(d => d.MaKH).WithMany(p => p.DonHangs)
//                .HasForeignKey(d => d.MaKh)
//                .HasConstraintName("FK_DonHang_KhachHang");

//            entity.HasOne(d => d.MaNvduyetNavigation).WithMany(p => p.DonHangMaNvduyetNavigations)
//                .HasForeignKey(d => d.MaNVDuyet)
//                .HasConstraintName("FK_DonHang_NhanVien");

//            entity.HasOne(d => d.MaNVGH).WithMany(p => p.DonHangMaNvghNavigations)
//                .HasForeignKey(d => d.MaNvgh)
//                .HasConstraintName("FK_DonHang_NhanVien1");

//            entity.HasOne(d => d.ShippingMethod).WithMany(p => p.DonHangs)
//                .HasForeignKey(d => d.ShippingMethodID)
//                .HasConstraintName("FK_DonHang_ShippingMethod");
//        });

//        modelBuilder.Entity<KhachHang>(entity =>
//        {
//            entity.HasKey(e => e.MaKH).HasName("PK__KhachHan__2725CF1EA6B6C0A4");

//            entity.ToTable("KhachHang");

//            entity.Property(e => e.MaKh).HasColumnName("MaKH");
//            entity.Property(e => e.CityId).HasColumnName("CityID");
//            entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");
//            entity.Property(e => e.DiaChi).HasMaxLength(200);
//            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
//            entity.Property(e => e.HoTen).HasMaxLength(100);
//            entity.Property(e => e.SoDienThoai)
//                .HasMaxLength(15)
//                .IsUnicode(false);
//            entity.Property(e => e.UserId)
//                .HasMaxLength(128)
//                .HasColumnName("UserID");
//            entity.Property(e => e.WardId).HasColumnName("WardID");

//            entity.HasOne(d => d.City).WithMany(p => p.KhachHangs)
//                .HasForeignKey(d => d.CityId)
//                .HasConstraintName("FK_KhachHang_Cities");

//            entity.HasOne(d => d.CustomerType).WithMany(p => p.KhachHangs)
//                .HasForeignKey(d => d.CustomerTypeId)
//                .HasConstraintName("FK_KhachHang_CustomType");

//            entity.HasOne(d => d.District).WithMany(p => p.KhachHangs)
//                .HasForeignKey(d => d.DistrictId)
//                .HasConstraintName("FK_KhachHang_Districts");

//            entity.HasOne(d => d.User).WithMany(p => p.KhachHangs)
//                .HasForeignKey(d => d.UserId)
//                .HasConstraintName("FK_KhachHang_AspNetUsers");

//            entity.HasOne(d => d.Ward).WithMany(p => p.KhachHangs)
//                .HasForeignKey(d => d.WardId)
//                .HasConstraintName("FK_KhachHang_Wards");
//        });

//        modelBuilder.Entity<LoaiSanPham>(entity =>
//        {
//            entity.HasKey(e => e.MaLsp).HasName("PK__LoaiSanP__3B983FFE53662336");

//            entity.ToTable("LoaiSanPham");

//            entity.Property(e => e.MaLsp).HasColumnName("MaLSP");
//            entity.Property(e => e.TenLsp)
//                .HasMaxLength(100)
//                .HasColumnName("TenLSP");
//        });

//        modelBuilder.Entity<Menu>(entity =>
//        {
//            entity.HasKey(e => e.MaMenu);

//            entity.ToTable("Menu");

//            entity.Property(e => e.NhomMenu)
//                .HasMaxLength(50)
//                .IsUnicode(false);

//            entity.HasOne(d => d.MaLoaiMenuNavigation).WithMany(p => p.Menus)
//                .HasForeignKey(d => d.MaLoaiMenu)
//                .HasConstraintName("FK_Menu_LoaiSanPham");

//            entity.HasOne(d => d.NhomMenuNavigation).WithMany(p => p.Menus)
//                .HasForeignKey(d => d.NhomMenu)
//                .HasConstraintName("FK_Menu_NhomMenu");
//        });

//        modelBuilder.Entity<NhanVien>(entity =>
//        {
//            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70AFFC0BF17");

//            entity.ToTable("NhanVien");

//            entity.Property(e => e.MaNv).HasColumnName("MaNV");
//            entity.Property(e => e.DiaChi).HasMaxLength(200);
//            entity.Property(e => e.HoTen).HasMaxLength(100);
//            entity.Property(e => e.SoDienThoai)
//                .HasMaxLength(15)
//                .IsUnicode(false);
//            entity.Property(e => e.UserId)
//                .HasMaxLength(128)
//                .HasColumnName("UserID");

//            entity.HasOne(d => d.User).WithMany(p => p.NhanViens)
//                .HasForeignKey(d => d.UserId)
//                .HasConstraintName("FK_NhanVien_AspNetUsers");
//        });

//        modelBuilder.Entity<NhomMenu>(entity =>
//        {
//            entity.HasKey(e => e.MaNhomMenu);

//            entity.ToTable("NhomMenu");

//            entity.Property(e => e.MaNhomMenu)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.TenNhomMenu).HasMaxLength(200);
//        });

//        modelBuilder.Entity<ProductAttribute>(entity =>
//        {
//            entity.HasKey(e => e.AttributeId).HasName("PK_Variant");

//            entity.ToTable("ProductAttribute");

//            entity.Property(e => e.AttributeId)
//                .HasMaxLength(20)
//                .IsUnicode(false)
//                .HasColumnName("AttributeID");
//            entity.Property(e => e.AttributeName).HasMaxLength(50);
//        });

//        modelBuilder.Entity<ProductVariant>(entity =>
//        {
//            entity.HasKey(e => e.VariantId).HasName("PK_ProductVariantStock");

//            entity.ToTable("ProductVariant");

//            entity.Property(e => e.VariantId).HasColumnName("VariantID");
//            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
//            entity.Property(e => e.ProductId).HasColumnName("ProductID");

//            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
//                .HasForeignKey(d => d.ProductId)
//                .OnDelete(DeleteBehavior.Cascade)
//                .HasConstraintName("FK_ProductVariant_SanPham");
//        });

//        modelBuilder.Entity<ProductVariantAttribute>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK_ProductVariantOption");

//            entity.ToTable("ProductVariantAttribute");

//            entity.Property(e => e.Id).HasColumnName("ID");
//            entity.Property(e => e.AttributeId)
//                .HasMaxLength(20)
//                .IsUnicode(false)
//                .HasColumnName("AttributeID");
//            entity.Property(e => e.AttributeValueId).HasColumnName("AttributeValueID");
//            entity.Property(e => e.VariantId).HasColumnName("VariantID");

//            entity.HasOne(d => d.Attribute).WithMany(p => p.ProductVariantAttributes)
//                .HasForeignKey(d => d.AttributeId)
//                .OnDelete(DeleteBehavior.Cascade)
//                .HasConstraintName("FK_ProductVariantAttribute_Attribute");

//            entity.HasOne(d => d.AttributeValue).WithMany(p => p.ProductVariantAttributes)
//                .HasForeignKey(d => d.AttributeValueId)
//                .HasConstraintName("FK_ProductVariantAttribute_AttributeValue");

//            entity.HasOne(d => d.Variant).WithMany(p => p.ProductVariantAttributes)
//                .HasForeignKey(d => d.VariantId)
//                .OnDelete(DeleteBehavior.Cascade)
//                .HasConstraintName("FK_ProductVariantAttribute_ProductVariant");
//        });

//        modelBuilder.Entity<SanPham>(entity =>
//        {
//            entity.HasKey(e => e.MaSp).HasName("PK__SanPham__2725081C91D0DE45");

//            entity.ToTable("SanPham");

//            entity.Property(e => e.MaSp).HasColumnName("MaSP");
//            entity.Property(e => e.Anh).HasMaxLength(100);
//            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 0)");
//            entity.Property(e => e.Dvt)
//                .HasMaxLength(10)
//                .HasColumnName("DVT");
//            entity.Property(e => e.MaLsp).HasColumnName("MaLSP");
//            entity.Property(e => e.MoTa).HasColumnType("ntext");
//            entity.Property(e => e.TenSp)
//                .HasMaxLength(100)
//                .HasColumnName("TenSP");

//            entity.HasOne(d => d.MaLspNavigation).WithMany(p => p.SanPhams)
//                .HasForeignKey(d => d.MaLsp)
//                .HasConstraintName("FK_SanPham_LoaiSanPham");
//        });

//        modelBuilder.Entity<ShippingRate>(entity =>
//        {
//            entity.HasKey(e => e.RateId);

//            entity.ToTable("ShippingRate");

//            entity.Property(e => e.RateId).HasColumnName("RateID");
//            entity.Property(e => e.CreateAt).HasColumnType("datetime");
//            entity.Property(e => e.FixedPrice).HasColumnType("decimal(18, 0)");
//            entity.Property(e => e.PricePerKm).HasColumnType("decimal(18, 0)");
//            entity.Property(e => e.ShippingMethodId).HasColumnName("ShippingMethodID");
//            entity.Property(e => e.WeightLitmit).HasColumnType("decimal(18, 0)");

//            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.ShippingRates)
//                .HasForeignKey(d => d.Region)
//                .HasConstraintName("FK_ShippingRate_Cities");

//            entity.HasOne(d => d.ShippingMethod).WithMany(p => p.ShippingRates)
//                .HasForeignKey(d => d.ShippingMethodId)
//                .HasConstraintName("FK_ShippingRate_ShippingMethod");
//        });

//        modelBuilder.Entity<Ward>(entity =>
//        {
//            entity.Property(e => e.WardId).HasColumnName("WardID");
//            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
//            entity.Property(e => e.WardName).HasMaxLength(50);

//            entity.HasOne(d => d.District).WithMany(p => p.Wards)
//                .HasForeignKey(d => d.DistrictId)
//                .HasConstraintName("FK_Wards_Districts");
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
