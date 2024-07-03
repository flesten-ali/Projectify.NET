using System.ComponentModel.DataAnnotations;

namespace Practice.Models
{
    public class ForgetPassword
    {


        [Key]
        [Required,EmailAddress, Display(Name ="Registred  Email address")]  
        public string Email { get; set; }
        public bool isEmailSent { get; set; }
 

    }
}
