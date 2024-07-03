using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualBasic;
using System.Net;
using System.Net.Mail;
namespace Practice.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var senderMail = "fbada858@gmail.com";
            var passward = "gzwhjhawvxulgyav";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                //sender 
                Credentials = new NetworkCredential(senderMail, passward)
            };
            var msg = new MailMessage(from: senderMail,
                                        to: email,
                                       subject,
                                       htmlMessage
                                       
                                    );

            msg.IsBodyHtml = true;
            msg.Body=htmlMessage;   
            return client.SendMailAsync( msg);
 
        }
    }
}
