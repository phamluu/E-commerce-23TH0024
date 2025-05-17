using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Models.Location;
using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Models.SystemSetting;
using E_commerce_23TH0024.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace E_commerce_23TH0024.Data
{
    public class SystemSettingDbContext : DbContext
    {
        public DbSet<NhomMenu> NhomMenus { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public SystemSettingDbContext(DbContextOptions<SystemSettingDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NhomMenu>(entity =>
            {
                entity.ToTable("NhomMenu");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TenNhomMenu).IsRequired().HasMaxLength(100);
                entity.HasMany(e => e.Menus);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LoaiMenu).IsRequired();
                entity.Property(e => e.MaLoaiMenu).IsRequired();
                entity.HasOne(e => e.NhomMenu)
                      .WithMany(p => p.Menus)
                      .HasForeignKey(e => e.IdNhomMenu);
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.ToTable("Configuration");
                entity.HasKey(e => e.Id);
            });
        }
    }

}
