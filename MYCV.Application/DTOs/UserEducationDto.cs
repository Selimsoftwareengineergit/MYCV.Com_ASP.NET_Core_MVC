using System.ComponentModel.DataAnnotations;
using MYCV.Domain.Enums;

namespace MYCV.Application.DTOs
{
    public class UserEducationDto
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public EducationLevel EducationLevel { get; set; }

        [Required]
        [MaxLength(150)]
        public string ExamName { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? InstituteName { get; set; }

        [MaxLength(100)]
        public string? BoardOrUniversity { get; set; }

        [MaxLength(100)]
        public string? GroupOrMajor { get; set; }

        [MaxLength(50)]
        public string? Result { get; set; }

        [MaxLength(50)]
        public string? Duration { get; set; }

        [Range(1900, 2100)]
        public int? PassingYear { get; set; }

        public bool IsCurrentlyStudying { get; set; }

        [MaxLength(250)]
        public string? Remarks { get; set; }
    }
}