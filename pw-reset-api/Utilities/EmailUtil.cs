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
        public static bool SendEmail(string emailAddr, string content)
        {
            return true;
            try
            {
                // Credentials
                var credentials = new NetworkCredential("peggy.00.zhang@gmail.com", "Zhg_740817");

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("190728@nttdata.com"),
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
    }    
}
