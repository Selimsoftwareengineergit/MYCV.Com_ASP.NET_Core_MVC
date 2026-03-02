using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MYCV.Domain.Enums;

namespace MYCV.Domain.Entities
{
    public class UserSkill : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required, MaxLength(100)]
        public string SkillName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Level { get; set; } = SkillLevel.Beginner.ToString();

        [Required, MaxLength(50)]
        public string SkillType { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Remarks { get; set; } = string.Empty;

        public double? YearsOfExperience { get; set; }

        [MaxLength(250)]
        public string? CertificateFile { get; set; }

        public int Priority { get; set; } = 1;

        public int? ProficiencyPercentage { get; set; }
    }
}