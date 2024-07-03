using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice.Models
{
    
    public class Project

    {
        [Key]
        public int projectId { get; set; }
        [Required]
        [Display(Name= "Title of the project")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Project discreption")]
        [MinLength(100,ErrorMessage = "Discreption must contain a least 100 character")]
        public string Discreption { get; set; }
         [Display(Name = "Upload your project as pdf file")]

        [NotMapped]
        
         public IFormFile PDFfile { get; set; }

        [Display(Name ="PDF")]
        public string? pdfURL { get; set; }
        [Display(Name ="Date of publish")]
        //[NotMapped]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateOfPublish { get; set; }

        //Relashionships with Category
        public Category Category { get; set; }
        [ForeignKey(nameof(Category))]
        [Required(ErrorMessage = "Project category is required")]
        [Display(Name = "Project category")]


        public int categoryId { get; set; }
        //Relashionships with User
        public ApplicationUser User { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string userId { get; set; }

    }
}
