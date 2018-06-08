using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RetrospectiveHelper.Models
{
    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} must contain at least {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeInfoBindingModel
    {
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Display(Name = "E-mail address")]
        public string Email { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "E-mail address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} must contain at least {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} must contain at least {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
