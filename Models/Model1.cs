using System.ComponentModel.DataAnnotations;

namespace Practice.Models
{
    public class Model1
    {
        [Key]  
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display (Name ="Display Order")]
        [Range(1,100,ErrorMessage ="Order can be between 1 and 100 only!")]
        public int DisplayOrder { get; set; }
        [Display(Name = "Date")]
        public DateTime CreatDateTime { get; set; }
     }
}
