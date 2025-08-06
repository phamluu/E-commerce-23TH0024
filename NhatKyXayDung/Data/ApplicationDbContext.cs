using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NhatKyXayDung.Models;

namespace NhatKyXayDung.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CongTrinh> CongTrinh { get; set; }
        public DbSet<NhatKy> NhatKy { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<CongTrinh>(entity =>
            {
                entity.ToTable("CongTrinh");
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.NhatKies).WithOne(e => e.CongTrinh).HasForeignKey(e => e.IdCongTrinh);
            });
            modelBuilder.Entity<NhatKy>(entity =>
            {
                entity.ToTable("NhatKy");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.CongTrinh).WithMany(e => e.NhatKies).HasForeignKey(e => e.IdCongTrinh);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
