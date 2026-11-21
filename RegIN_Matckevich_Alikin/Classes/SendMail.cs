using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RegIN_Matckevich_Alikin.Classes
{
    public class SendMail
    {
        public static void SendMessage(string Message, string To)
        {
            var smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("h4tache@yandex.ru", "password"),
                EnableSsl = true
            };
            smtpClient.Send("h4tache@yandex.ru", To, "Проект RegIn", Message);
        }
    }
}
