using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.DTOs
{
    public class UserSkillDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string SkillName { get; set; } = string.Empty;

        public string Level { get; set; } = string.Empty;

        public string SkillType { get; set; } = string.Empty;

        public string Remarks { get; set; } = string.Empty;

        public double? YearsOfExperience { get; set; }

        public string? CertificateFile { get; set; }

        public int Priority { get; set; }

        public int? ProficiencyPercentage { get; set; }
    }
}