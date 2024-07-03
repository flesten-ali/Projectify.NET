using Practice.Db;
using Practice.Email;
using Practice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 using System.Net.Mail;
//using System.Web.Helpers;

namespace Practice.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
       // private readonly ApplicationDbContext _db;
        private readonly AuthDbContext _Adb;
        private readonly EmailSender _emailSender;
        private readonly IWebHostEnvironment _env;






        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,  AuthDbContext adb, EmailSender emailSender, IWebHostEnvironment env)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            //_db = db;
            _Adb = adb;
            _emailSender = emailSender;
            _env = env;
        }


        //Get
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        //Get
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(Account registerINFO)
        {

            if (registerINFO != null)
            {
                if (ModelState.IsValid)
                {




                    //creat identity for this user
                    var IdentityUser = new ApplicationUser
                    {
                        UserName = registerINFO.Username,
                        Email = registerINFO.Email

                    };


                    //if Creating user identity successed then
                    var identityResult = await userManager.CreateAsync(IdentityUser, registerINFO.Password);
                    if (identityResult.Succeeded)
                    {
                        // assign the role for this user 
                        var userRole = await userManager.AddToRoleAsync(IdentityUser, "User");
                        // if assigning the role successded then the user is registed successfully 
                        if (userRole.Succeeded)
                        {
                            // show the success notification 
                            return RedirectToAction("Index", "Home");
                            //return RedirectToAction("Register");
                        }

                    }
                     
                    else
                    {

                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(registerINFO);

                    }

                }

                //try to register again 
                return View(registerINFO);
            }
            return View();

        }


        public IActionResult Login(string ReturnURL)
        {
            //assign the value of the(url that direct me to the login)
            var LoginModel = new Login()
            {
                ReturnURL = ReturnURL
            };
            return View(LoginModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(Login loginINFO)
        {
            if (loginINFO != null)
            {
                if (ModelState.IsValid)
                {
                    var SignInResult = await signInManager.PasswordSignInAsync(loginINFO.Username, loginINFO.Password, false, false);

                    if (SignInResult.Succeeded)
                    {
                        if (!string.IsNullOrWhiteSpace(loginINFO.ReturnURL))
                        {
                            return Redirect(loginINFO.ReturnURL);

                        }
                        TempData["sucess"] = "Welcome " + loginINFO.Username;

                        return RedirectToAction("Index", "Home");
                    }

                    TempData["access"] = "Username or password is wrong...";

                    return View(loginINFO);
                }
                return View(loginINFO);
            }
            return View();

        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied(string ReturnURL)
        {
            TempData["access"] = "you are not allowed to acsses " + ReturnURL + " page";
            return RedirectToAction("Index", "Home");
            // return View();
        }


        public IActionResult ForgetPassword()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPassword model)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                model.isEmailSent = true;
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // 
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    //Creat Link
                    var forgorPasswordlink = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

                    ////send email
                    var emailHtmlPath = Path.Combine(_env.ContentRootPath, "Email", "email.html");
                    var htmlBody = await System.IO.File.ReadAllTextAsync(emailHtmlPath);
                    htmlBody = htmlBody.Replace("{forgotPasswordLink}", forgorPasswordlink);

                    await _emailSender.SendEmailAsync(model.Email, "Forgot Password Link", htmlBody);

                }

            }
            return View(model);

        }


        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPassword { Token = token, Email = email });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                }
                return View(model);

            }
            return View(model);

        }
    }
}
