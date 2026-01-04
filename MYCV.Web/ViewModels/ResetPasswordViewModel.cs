using System.ComponentModel.DataAnnotations;

namespace MYCV.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="Email address is required")]
        [EmailAddress(ErrorMessage ="Please enter a valid email address")]
        [Display(Name = "Email Address")]
        [StringLength(100, ErrorMessage ="Email cannot exceed 100 characters")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage ="Email address is required")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public int PasswordStrength { get; set; }
        public DateTime TokenExpiryTime { get; set; }
    }
}