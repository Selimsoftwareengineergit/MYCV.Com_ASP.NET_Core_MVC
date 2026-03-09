using MYCV.Domain.Enums;

namespace MYCV.Application.DTOs
{
    public class UserSkillDto
    {
        /// <summary>
        /// Unique identifier of the skill record
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Associated user ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Name of the skill
        /// </summary>
        public string SkillName { get; set; } = string.Empty;

        /// <summary>
        /// Level of the skill (Beginner, Intermediate, Expert, Master)
        /// </summary>
        public SkillLevel Level { get; set; } = SkillLevel.Beginner;

        /// <summary>
        /// Type of skill (Technical, Soft, Language, Tool, Other)
        /// </summary>
        public SkillType SkillType { get; set; } = SkillType.Technical;

        /// <summary>
        /// Optional remarks about the skill
        /// </summary>
        public string? Remarks { get; set; }

        /// <summary>
        /// Number of years of experience for this skill
        /// </summary>
        public double? YearsOfExperience { get; set; }

        /// <summary>
        /// Priority of the skill (used for ordering/display)
        /// </summary>
        public int Priority { get; set; } = 1;

        /// <summary>
        /// Optional proficiency percentage (0-100)
        /// </summary>
        public int? ProficiencyPercentage { get; set; }
    }
}