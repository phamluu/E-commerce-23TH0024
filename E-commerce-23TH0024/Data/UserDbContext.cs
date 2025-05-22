using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Models.Identity;
using E_commerce_23TH0024.Models.Location;
using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace E_commerce_23TH0024.Data
{
    public class UserDbContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<NhanVien> NhanViens { get; set; }
        
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.ToTable("NhanVien");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.AspNetUser).WithOne(u => u.NhanVien)
                .HasForeignKey<NhanVien>(e => e.IdAspNetUsers);
            });
            base.OnModelCreating(modelBuilder);
        }
    }

    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            optionsBuilder.UseSqlServer("Server=ADMIN\\SQLEXPRESS;Database=E_commerce_23TH0024;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true");

            return new UserDbContext(optionsBuilder.Options);
        }
    }
}
