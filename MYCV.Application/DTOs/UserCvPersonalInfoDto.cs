namespace MYCV.Application.DTOs
{
    public class UserCvPersonalInfoDto
    {
        public int UserId { get; set; }
        // Personal Information
        public string FullName { get; set; } = null!;
        public string ProfessionalTitle { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;

        // Contact Information
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;

        // Social & Online Presence
        public string? LinkedIn { get; set; }
        public string? GitHub { get; set; }
        public string? Portfolio { get; set; }
        public string? Website { get; set; }

        // Optional Professional Detail
        public string? LinkedInHeadline { get; set; }
        public string? Summary { get; set; }
        public string? ProfilePictureUrl { get; set; } 
    }
}