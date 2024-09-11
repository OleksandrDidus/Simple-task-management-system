using Microsoft.EntityFrameworkCore;
using Api.Data.Model;
using System.Collections.Generic;
using System.Reflection.Emit;
using TaskStatus = Api.Data.Model.TaskStatus;

namespace Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tasks> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.PasswordHash).IsRequired();
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired();
                entity.Property(t => t.Status).HasDefaultValue(TaskStatus.Pending);
                entity.Property(t => t.Priority).HasDefaultValue(TaskPriority.Medium);
                entity.HasOne(t => t.User)
                      .WithMany(u => u.Tasks)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }


}
