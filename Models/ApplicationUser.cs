using Microsoft.AspNetCore.Identity;

namespace Practice.Models
{
    public class ApplicationUser : IdentityUser
    {

         public List<Project> projects { get; set; }
    }
}
