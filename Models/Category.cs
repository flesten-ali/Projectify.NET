using System.ComponentModel.DataAnnotations;

namespace Practice.Models
{
    public class Category
    {

        [Key]
        public int categoryId { get; set; }
        [Display(Name="Project category")]
        [Required(ErrorMessage ="Please select the project category")]
        
         public string category { get; set; }
        //Relashioships
        public List<Project> Projects { get; set;}
    }
}
