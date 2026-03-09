using MYCV.Domain.Enums;

namespace MYCV.Application.DTOs
{
    public class UserExperienceDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Company { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string? Department { get; set; }

        public string? Location { get; set; }

        public EmploymentType EmploymentType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Responsibilities { get; set; }

        public string? Remarks { get; set; }

        public string? ProjectLink { get; set; }

        public int Priority { get; set; }
    }
}