using System.ComponentModel.DataAnnotations;

namespace Practice.dto
{
    public class dContactModel
    {
        [Key]
        public int Id { get; set; }



        [Required(ErrorMessage = "Subject Field is required")]
        public string Subject { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email Field is required")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Massage Field is required")]
        public string Massage { get; set; }


        public int CategroyId { get; set; } 
    }
}
