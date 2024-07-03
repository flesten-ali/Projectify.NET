using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Practice.Models
{
    public class ResetPassword
    {
        [Key]
         [EmailAddress]
        [Required(ErrorMessage = "please enter your email!")]
        public string Email { get; set; }


        [Display(Name ="New Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "please enter your password!")]
        public string NewPassword { get; set; }




        [DataType(DataType.Password)]
        [Required(ErrorMessage = "confirm password required!")]
        [CompareAttribute("NewPassword", ErrorMessage = "Password doesn't match.")]
        [NotMapped]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [NotNull]
        public string Token { get; set; }


    }
}
