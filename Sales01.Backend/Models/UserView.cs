namespace Sales01.Backend.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;
    using Sales01.Domain.Models;

    [NotMapped]
    public class UserView : User
    {
        [Display(Name = "New Photo")]
        public HttpPostedFileBase LifeLogo { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(20, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characteres",
        MinimumLength = 8)]
        [Display(Name = "Passwords")]
        [DataType(DataType.Password)]
        public string Passwords { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(20, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characteres",
        MinimumLength = 8)]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Passwords", ErrorMessage = "Password and confirm does not match....!")]
        public string ConfirmPassword { get; set; }

        [StringLength(20, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characteres",
        MinimumLength = 8)]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}