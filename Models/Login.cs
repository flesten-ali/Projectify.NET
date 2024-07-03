using System.ComponentModel.DataAnnotations;

namespace Practice.Models
{
    public class Login
    {

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "please enter your password!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "please enter your username!")]
        public string Username  { get; set; }
        public string?  ReturnURL { get; set; }
    }
}
