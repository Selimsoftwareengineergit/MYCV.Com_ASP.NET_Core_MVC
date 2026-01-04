using System.ComponentModel.DataAnnotations;

namespace MYCV.Web.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage ="Email address is required")]
        [EmailAddress(ErrorMessage ="Please enter a valid email address")]
        [Display(Name ="Email Address")]
        [StringLength(100,ErrorMessage = "Email can not exceed 100 characters")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Security Question")]
        public string SecurityAnswer { get; set; } = string.Empty;

        [Display(Name = "I'm not a robot")]
        public bool IsHuman { get; set; }
    }
}