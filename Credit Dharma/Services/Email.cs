using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Credit_Dharma.Services
{
    static public class Email
    {
        public static void SendEmail(string account ,string email,string message)
        {
            var smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("creditdharma@outlook.com", "Zxc@12345"),
                EnableSsl = true,
                
            };

            smtpClient.Send("creditdharma@outlook.com", email, "Estatus de cuenta "+account, message);
        }
    }
}
