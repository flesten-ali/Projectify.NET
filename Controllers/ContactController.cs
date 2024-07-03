using Practice.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Practice.Email;
using Practice.Db;
using Microsoft.AspNetCore.Identity;

namespace Practice.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailSender emailSender;
         
        private readonly IWebHostEnvironment _env;

        public ContactController(IEmailSender emailSender, IWebHostEnvironment env)
        {
            this.emailSender = emailSender;
            _env = env;
        }

        [HttpGet]
        public IActionResult ContactEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactEmail(ContactModel contact )
        {
            if(ModelState.IsValid) {

                var emailHtmlPath = Path.Combine(_env.ContentRootPath, "Email", "ContactEmail.html");
                var htmlBody = await System.IO.File.ReadAllTextAsync(emailHtmlPath);
                htmlBody = htmlBody.Replace("{Subject}", contact.Subject);
                htmlBody = htmlBody.Replace("{Email}", contact.Email);
                htmlBody = htmlBody.Replace("{Message}", contact.Massage );

                await emailSender.SendEmailAsync("flesten.ali@gmail.com", "Project Hup Massege", htmlBody);
                TempData["sucess"] = "Thanks for your email.we will contact you soon.";
                ModelState.Clear();
                return View();


            }

            return View(contact);

        }


        public async Task<IActionResult> ContactMeEmail(ContactModel contact)
        {
            if (ModelState.IsValid)
            {

                var emailHtmlPath = Path.Combine(_env.ContentRootPath, "Email", "ContactEmail.html");
                var htmlBody = await System.IO.File.ReadAllTextAsync(emailHtmlPath);
                htmlBody = htmlBody.Replace("{Subject}", contact.Subject);
                htmlBody = htmlBody.Replace("{Email}", contact.Email);
                htmlBody = htmlBody.Replace("{Message}", contact.Massage);

                await emailSender.SendEmailAsync("flesten.ali@gmail.com", "Project Hup Massege", htmlBody);
                TempData["sucess"] = "Thanks for your email.we will contact you soon.";
                ModelState.Clear();

                return RedirectToAction("Index", "Home");


            }
            return RedirectToAction("Index", "Home");
        }
    }
}
