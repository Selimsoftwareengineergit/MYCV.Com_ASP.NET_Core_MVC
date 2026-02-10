using Microsoft.EntityFrameworkCore;
using MYCV.Domain.Entities;

namespace MYCV.Infrastructure.Data
{
    public class MyCvDbContext : DbContext
    {
        public MyCvDbContext(DbContextOptions<MyCvDbContext> options) : base(options) { }

        // DbSets
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserPersonalDetail> UserPersonalDetails { get; set; } = null!;
        public DbSet<UserEducation> UserEducations { get; set; } = null!;
        public DbSet<UserExperience> UserExperiences { get; set; } = null!;
        public DbSet<UserSkill> UserSkills { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → UserEducation (one-to-many)
            modelBuilder.Entity<UserEducation>()
                .HasOne(e => e.User)
                .WithMany(u => u.UserEducations)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // User → UserExperience (one-to-many)
            modelBuilder.Entity<UserExperience>()
                .HasOne(e => e.User)
                .WithMany(u => u.UserExperiences)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // User → UserSkill (one-to-many)
            modelBuilder.Entity<UserSkill>()
                .HasOne(s => s.User)
                .WithMany(u => u.UserSkills)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // User → UserPersonalDetail (one-to-one)
            modelBuilder.Entity<UserPersonalDetail>()
                .HasOne(pd => pd.User)
                .WithOne(u => u.UserPersonalDetails)
                .HasForeignKey<UserPersonalDetail>(pd => pd.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: Configure max length, required, etc., if needed
            modelBuilder.Entity<UserEducation>()
                .Property(e => e.EducationLevel)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<UserExperience>()
                .Property(e => e.Company)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<UserSkill>()
                .Property(s => s.SkillName)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}