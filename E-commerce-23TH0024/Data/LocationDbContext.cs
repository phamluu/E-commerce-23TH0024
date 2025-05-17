using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Models.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace E_commerce_23TH0024.Data
{
    public class LocationDbContext: DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }

        public LocationDbContext(DbContextOptions<LocationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("Cities");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CityName).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("Districts");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DistrictName).IsRequired().HasMaxLength(100);
                entity.HasOne(e => e.City)
                      .WithMany(p => p.Districts)
                      .HasForeignKey(e => e.IdCity);
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.ToTable("Wards");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WardName).IsRequired().HasMaxLength(100);
                entity.HasOne(e => e.District)
                      .WithMany(d => d.Wards)
                      .HasForeignKey(e => e.IdDistrict);
            });
        }
    }

    public class LocationDbContextFactory : IDesignTimeDbContextFactory<LocationDbContext>
    {
        public LocationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LocationDbContext>();

            // Thay bằng chuỗi kết nối thật của bạn
            //optionsBuilder.UseSqlServer("Server=ADMIN\\SQLEXPRESS;Database=E_commerce_23TH0024;Trusted_Connection=True;TrustServerCertificate=True;");
            optionsBuilder.UseSqlServer("Server=ADMIN\\SQLEXPRESS;Database=E_commerce_23TH0024;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true");
            return new LocationDbContext(optionsBuilder.Options);
        }
    }


}
