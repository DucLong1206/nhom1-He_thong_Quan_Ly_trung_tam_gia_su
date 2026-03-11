using System.Net;
using System.Net.Mail;

public class EmailService
{
    public bool SendMail(string toEmail, string subject, string body)
    {
        try
        {
            var fromEmail = "lhqeducation@gmail.com";
            var fromPass = "swpyhvekymdcllkx";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromEmail);
            message.To.Add(toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(fromEmail, fromPass);
            smtp.EnableSsl = true;

            smtp.Send(message);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}