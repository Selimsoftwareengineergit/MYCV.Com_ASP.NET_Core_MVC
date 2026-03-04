using Microsoft.EntityFrameworkCore;
using MYCV.Domain.Entities;

namespace MYCV.Infrastructure.Data
{
    public class MyCvDbContext : DbContext
    {
        public MyCvDbContext(DbContextOptions<MyCvDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserPersonalDetail> UserPersonalDetails { get; set; } = null!;
        public DbSet<UserEducation> UserEducations { get; set; } = null!;
        public DbSet<UserExperience> UserExperiences { get; set; } = null!;
        public DbSet<UserSkill> UserSkills { get; set; } = null!;
        public DbSet<UserProject> UserProjects { get; set; } = null!;
        public DbSet<UserLanguage> UserLanguages { get; set; } = null!;
        public DbSet<UserSummaryObjective> UserSummaryObjectives { get; set; } = null!;
        public DbSet<UserReference> UserReferences { get; set; } = null!;
        public DbSet<UserSubscription> UserSubscriptions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → UserPersonalDetail (One-to-One)
            modelBuilder.Entity<UserPersonalDetail>()
                .HasOne(pd => pd.User)
                .WithOne(u => u.UserPersonalDetail)
                .HasForeignKey<UserPersonalDetail>(pd => pd.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → UserEducation (One-to-Many)
            modelBuilder.Entity<UserEducation>()
                .HasOne(e => e.User)
                .WithMany(u => u.UserEducations)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // User → UserExperience (One-to-Many)
            modelBuilder.Entity<UserExperience>()
                .HasOne(e => e.User)
                .WithMany(u => u.UserExperiences)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // User → UserSkill (One-to-Many)
            modelBuilder.Entity<UserSkill>()
                .HasOne(s => s.User)
                .WithMany(u => u.UserSkills)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // User → UserProject (One-to-Many)
            modelBuilder.Entity<UserProject>()
                .HasOne(p => p.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // User → UserLanguage (One-to-Many)
            modelBuilder.Entity<UserLanguage>()
                .HasOne(l => l.User)
                .WithMany(u => u.UserLanguages)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // User → UserSummaryObjective (One-to-Many)
            modelBuilder.Entity<UserSummaryObjective>()
                .HasOne(s => s.User)
                .WithMany(u => u.UserSummaryObjectives)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // User → UserReference (One-to-Many)
            modelBuilder.Entity<UserReference>()
                .HasOne(r => r.User)
                .WithMany(u => u.UserReferences)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // User → UserSubscription (One-to-Many)
            modelBuilder.Entity<UserSubscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.UserSubscriptions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);


            // Unique Email for Users
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Optional: Composite index for priority sorting
            modelBuilder.Entity<UserSkill>()
                .HasIndex(s => new { s.UserId, s.Priority });

            modelBuilder.Entity<UserExperience>()
                .HasIndex(e => new { e.UserId, e.Priority });

            modelBuilder.Entity<UserProject>()
                .HasIndex(p => new { p.UserId, p.Priority });

            modelBuilder.Entity<UserLanguage>()
                .HasIndex(l => new { l.UserId, l.Priority });

            modelBuilder.Entity<UserSummaryObjective>()
                .HasIndex(s => new { s.UserId, s.Priority });

            modelBuilder.Entity<UserReference>()
                .HasIndex(r => new { r.UserId, r.Priority });

            modelBuilder.Entity<UserSubscription>()
                .HasIndex(s => new { s.UserId, s.StartDate });
        }
    }
}