using System.ComponentModel.DataAnnotations;

namespace RetrospectiveHelper.Models
{
    public class AddUserBindingModel
    {
        [Required]
        [Display(Name = "E-mail address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Project ID")]
        public int ProjectId { get; set; }
    }

    public class LeaveBindingModel
    {
        [Required]
        [Display(Name = "Project ID")]
        public int ProjectId { get; set; }
    }

}