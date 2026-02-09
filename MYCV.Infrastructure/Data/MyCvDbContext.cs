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

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserCv> UserCvs { get; set; } = null!;
        public DbSet<UserEducation> UserEducations { get; set; } = null!;
        public DbSet<UserExperiences> UserExperiences { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Language> Languages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → UserEducation (one-to-many)
            modelBuilder.Entity<UserEducation>()
                    .HasOne(e => e.User)
                    .WithMany(u => u.UserEducations)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.ClientCascade);

            // User → UserCv (one-to-one)
            modelBuilder.Entity<UserCv>()
                .HasOne(cv => cv.User)
                .WithOne(u => u.UserCv)
                .HasForeignKey<UserCv>(cv => cv.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserCv → related CV entities (one-to-many)
            modelBuilder.Entity<UserExperiences>()
                .HasOne(e => e.UserCv)
                .WithMany(cv => cv.UserExperiences)
                .HasForeignKey(e => e.UserCvId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Skill>()
                .HasOne(s => s.UserCv)
                .WithMany(cv => cv.Skills)
                .HasForeignKey(s => s.UserCvId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.UserCv)
                .WithMany(cv => cv.Projects)
                .HasForeignKey(p => p.UserCvId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Language>()
                .HasOne(l => l.UserCv)
                .WithMany(cv => cv.Languages)
                .HasForeignKey(l => l.UserCvId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}