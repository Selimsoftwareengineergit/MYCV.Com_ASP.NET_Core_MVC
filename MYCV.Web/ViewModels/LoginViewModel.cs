using System.ComponentModel.DataAnnotations;

namespace MYCV.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        [Display(Name = "Verification Code")]
        public string? VerificationCode { get; set; }

        public string? ReturnUrl { get; set; }
    }
}