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

        [Required]
        public SkillLevel Level { get; set; } = SkillLevel.Beginner;

        [Required]
        public SkillType SkillType { get; set; } = SkillType.Technical;

        [MaxLength(250)]
        public string? Remarks { get; set; }

        public double? YearsOfExperience { get; set; }

        public int Priority { get; set; } = 1;

        [Range(0, 100)]
        public int? ProficiencyPercentage { get; set; }
    }
}