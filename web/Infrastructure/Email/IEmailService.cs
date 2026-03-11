namespace He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Email;

public interface IEmailService
{
    bool SendMail(string toEmail, string subject, string body);
}
