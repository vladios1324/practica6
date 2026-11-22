using System.Net;
using System.Net.Mail;

namespace RegIN_Matckevich_Alikin.Classes
{
    public class SendMail
    {
        public static void SendMessage(string Message, string To)
        {
            var smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("h4tache@yandex.ru", "bnvapkocietxnqvz"),
                EnableSsl = true
            };
            smtpClient.Send("h4tache@yandex.ru", To, "Проект RegIn", Message);
        }
    }
}
