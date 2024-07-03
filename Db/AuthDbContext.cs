using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Practice.Models;
using System.Reflection.Emit;

namespace Practice.Db
{
    public class AuthDbContext : IdentityDbContext
    {
        public DbSet<Project> projects { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<ApplicationUser> users { get; set; }
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }
 
         
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //specify the relations project-category
        builder.Entity<Project>().HasOne(b => b.Category).WithMany(a => a.Projects)
        .HasForeignKey(b => b.categoryId);
        //specify the relations project-user
        builder.Entity<Project>().HasOne(b => b.User).WithMany(a => a.projects)
            .HasForeignKey(b => b.userId);

        //Creat List of Roles
        var AdminRoleId = "07015d91-67f0-4953-97bf-c2a5d0d07325";
        var SuperAdminRoleId = "86e505ed-919e-4612-bffb-08b97e7de0c9";
        var UserRoleId = "f43b94be-2309-4703-9d16-5c718a1262e5";

        var roles = new List<IdentityRole> {
              new IdentityRole
             {
                  Name= "Admin",
                  NormalizedName= "Admin",
                  Id=AdminRoleId,
                  ConcurrencyStamp=AdminRoleId

             },
                 new IdentityRole
             {
                  Name= "SuperAdmin",
                  NormalizedName= "SuperAdmin",
                  Id=SuperAdminRoleId,
                  ConcurrencyStamp=SuperAdminRoleId

             },
               new IdentityRole
             {
                  Name= "User",
                  NormalizedName= "User",
                  Id=UserRoleId,
                  ConcurrencyStamp=UserRoleId
             }
             };
        //seeding roles in the datebase
        builder.Entity<IdentityRole>().HasData(roles);
        //creat SuperAdmin User
        var SuperAdminUserId = "2582621b-3de2-4255-9766-aaae346c85c7";
        var SuperAdminUser = new IdentityUser
        {
            UserName = "flesten.ali@gmail.com",
            Email = "flesten.ali@gmail.com",
            NormalizedUserName = "flesten.ali@gmail.com".ToUpper(),
            NormalizedEmail = "flesten.ali@gmail.com".ToUpper(),
            Id = SuperAdminUserId
        };
        //Creat Passward for superAdminUser
        SuperAdminUser.PasswordHash =
         new PasswordHasher<IdentityUser>().HashPassword(SuperAdminUser, "Falo123_");
        //seed SuperAdminUser in the database
        builder.Entity<IdentityUser>().HasData(SuperAdminUser);


        //Give all the Rolles to the SuperAdminUSer
        var SuperAdminRoles = new List<IdentityUserRole<string>> {

                new IdentityUserRole<string> {
                    UserId= SuperAdminUserId,
                    RoleId=AdminRoleId

                },
                 new IdentityUserRole<string> {
                    UserId= SuperAdminUserId,
                    RoleId=SuperAdminRoleId


                }, new IdentityUserRole<string> {
                    UserId= SuperAdminUserId,
                    RoleId=UserRoleId


                }
            };

        //seed SuperAdmin With his Roles
        builder.Entity<IdentityUserRole<string>>().HasData(SuperAdminRoles);

        //creat categories + seed them
        builder.Entity<Category>().HasData(
       new Category { categoryId = 1, category = "Medical" },
       new Category { categoryId = 2, category = "Computer Engineering" },
       new Category { categoryId = 3, category = "Industrial" },
       new Category
       {
           categoryId = 4,
           category = "Commercial"

       }
   );
    }
    public DbSet<Practice.Models.ForgetPassword> ForgetPassword { get; set; }
    public DbSet<Practice.Models.ResetPassword> ResetPassword { get; set; }
    public DbSet<Practice.Models.ContactModel> ContactModel { get; set; }
}
}
