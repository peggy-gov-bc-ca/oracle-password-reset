using common;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
//using MailKit.Net.Smtp;
//using MimeKit;

namespace pw_reset_api.Utilities
{
    public class EmailUtil
    {
        public static bool SendEmailWithGoogle(string emailAddr, string content)
        {
            //return true;
            try
            {
                // Credentials
                var credentials = new NetworkCredential(Constants.EMAIL_ADDRESS, Constants.EMAIL_PASSWORD);

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress(Constants.EMAIL_ADDRESS),
                    Subject = "Oracle Password Reset Verification Code",
                    Body = content
                };

                mail.IsBodyHtml = true;
                //mail.To.Add(new MailAddress("190728@nttdata.com"));
                mail.To.Add(new MailAddress(emailAddr));

                // Smtp client
                var client = new System.Net.Mail.SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };

                client.Send(mail);

                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }

        }

        public static async Task<bool> SendEmailAsyncWithSendGrid(string emailAddr, string content)
        {
            string apiKey = "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";
            SendGridClient client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("support@sierrasystem.com", "Support Team"),
                Subject = "Oracle Server Password Reset - Verification Code",
                PlainTextContent = content,
                HtmlContent = content
            };
            msg.AddTo(new EmailAddress(emailAddr, "Peggy"));
            var response = await client.SendEmailAsync(msg);
            return true;     
        }
    }    
}
