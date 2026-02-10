using MYCV.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsEmailVerified { get; set; } = false;
        public string? VerificationCode { get; set; }
        public UserRole Role { get; set; } = UserRole.Client;

        // Navigation properties
        public virtual ICollection<UserEducation> UserEducations { get; set; } = new List<UserEducation>();
        public virtual ICollection<UserExperience> UserExperiences { get; set; } = new List<UserExperience>();
        public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
        public virtual UserPersonalDetail? UserPersonalDetails { get; set; }
    }
}