using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYCV.Application.DTOs
{
    public class UserPersonalDetailDto
    {
        public int Id { get; set; }

        // User Reference
        public int UserId { get; set; }

        // Personal Information
        public string FullName { get; set; } = null!;
        public string ProfessionalTitle { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        // Contact Information
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Address { get; set; }

        // Professional Summary
        public string? Summary { get; set; }
        public string? Objective { get; set; }
      

        // Social & Online Presence
        public string? LinkedIn { get; set; }
        public string? GitHub { get; set; }
        public string? Portfolio { get; set; }
        public string? Website { get; set; }
        public string? LinkedInHeadline { get; set; }

        // ✅ For Upload
        public IFormFile? ProfilePicture { get; set; }

        // ✅ For Returning Saved URL
        public string? ProfilePictureUrl { get; set; }
    }
}