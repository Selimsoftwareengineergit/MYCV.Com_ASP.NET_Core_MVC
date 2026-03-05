using Microsoft.AspNetCore.Http;
using MYCV.Domain.Enums;

namespace MYCV.Application.DTOs
{
    public class UserPersonalDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string ProfessionalTitle { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public Religion? Religion { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Country { get; set; } = "Bangladesh";
        public string City { get; set; } = null!;
        public string? PresentAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? LinkedIn { get; set; }
        public string? GitHub { get; set; }
        public string? Portfolio { get; set; }
        public string? Website { get; set; }
        public string? LinkedInHeadline { get; set; }
        public string? Nationality { get; set; } = "Bangladeshi";
    }
}