using Microsoft.EntityFrameworkCore;
using MYCV.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Infrastructure.Data
{
    public class MyCvDbContext : DbContext
    {
        public MyCvDbContext(DbContextOptions<MyCvDbContext> options) : base(options) { }

        // Existing Users table
        public DbSet<User> Users { get; set; } = null!;

        // New CV-related tables
        public DbSet<UserCv> UserCvs { get; set; } = null!;
        public DbSet<UserEducation> UserEducations { get; set; } = null!;
        public DbSet<UserExperiences> UserExperiences { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Language> Languages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationships: UserCv → Related entities

            // UserEducation
            modelBuilder.Entity<UserEducation>()
                .HasOne(e => e.UserCv)
                .WithMany(u => u.UserEducations)
                .HasForeignKey(e => e.UserCvId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserExperiences
            modelBuilder.Entity<UserExperiences>()
                .HasOne(w => w.UserCv)
                .WithMany(u => u.UserExperiences)  
                .HasForeignKey(w => w.UserCvId)
                .OnDelete(DeleteBehavior.Cascade);

            // Skills
            modelBuilder.Entity<Skill>()
                .HasOne(s => s.UserCv)
                .WithMany(u => u.Skills)
                .HasForeignKey(s => s.UserCvId)
                .OnDelete(DeleteBehavior.Cascade);

            // Projects
            modelBuilder.Entity<Project>()
                .HasOne(p => p.UserCv)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.UserCvId)
                .OnDelete(DeleteBehavior.Cascade);

            // Languages
            modelBuilder.Entity<Language>()
                .HasOne(l => l.UserCv)
                .WithMany(u => u.Languages)
                .HasForeignKey(l => l.UserCvId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
