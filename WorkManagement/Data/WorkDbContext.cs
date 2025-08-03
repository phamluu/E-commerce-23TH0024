using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkManagement.Entities;

namespace WorkManagement.Data
{
    public class WorkDbContext: DbContext
    {
        
        public WorkDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskRead> TaskReads { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Tasks).WithOne(e => e.Project).HasForeignKey(e => e.IdProject);
            });
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.ToTable("Task");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Project).WithMany(e => e.Tasks).HasForeignKey(e => e.IdProject);
            });
            modelBuilder.Entity<TaskRead>(entity =>
            {
                entity.ToTable("TaskRead");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Task).WithMany(e => e.TaskReads).HasForeignKey(e => e.IdTask);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
