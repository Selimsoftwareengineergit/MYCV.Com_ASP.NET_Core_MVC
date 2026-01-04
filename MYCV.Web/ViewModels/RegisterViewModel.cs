using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MYCV.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "First Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength =6, ErrorMessage ="Password must be at least 6 chracters")]
        [Display(Name="Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage ="Please confirm your password")]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password", ErrorMessage ="Password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "I agree to the Terms and Conditions")]
        [Range(typeof(bool),"true", "true", ErrorMessage ="You must accept the terms and condition")]
        public bool AccetTerms { get; set; }
    }
}