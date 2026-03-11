using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Email;

[Obsolete("Use IEmailService via DI instead")]
public class EmailService
{
    private readonly IEmailService _emailService;

    public EmailService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public bool SendMail(string toEmail, string subject, string body) => _emailService.SendMail(toEmail, subject, body);
}
