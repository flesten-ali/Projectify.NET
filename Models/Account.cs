using Practice.Db;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Practice.Models
{
 
    public class Account : IValidatableObject
    {
       
        [Key][Column(Order = 0)]
        public string id { get; set; }




        [Key]
        [Column(Order = 1)]
        [EmailAddress]
        [Required(ErrorMessage = "please enter your email!")]
        public string Email { get; set; }


        [Required(ErrorMessage = "please enter your username!")]
        public string Username { get; set; }




        [Required(ErrorMessage = "please enter your university name!")]
        public string University { get; set; }




        [DataType(DataType.Password)]
        [Required(ErrorMessage = "please enter your password!")]
        public string Password { get; set; }





        [DataType(DataType.Password)]
        [Required(ErrorMessage = "confirm password required!")]
        [CompareAttribute("Password", ErrorMessage = "Password doesn't match.")]
        [NotMapped]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; set; }






        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var dbContext = (AuthDbContext)validationContext.GetService(typeof(AuthDbContext));
            if (dbContext.Users.Any(c => (!c.Id.Equals(id)) && c.Email.Equals(Email)))
            {
                yield return new ValidationResult("This Email is already exists.", new[] { nameof(Email) });
            }
        }








    }
}
