using He_thong_Quan_Ly_trung_tam_gia_su.Application.DTOs;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Email;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Repositories;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Security;
using He_thong_Quan_Ly_trung_tam_gia_su.Models;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Application.Services;

public class AuthService(ITaiKhoanRepository taiKhoanRepository, IUserRepository userRepository, IEmailService emailService, ILogger<AuthService> logger) : IAuthService
{
    public ApiResponse<AuthenticatedUser> Login(LoginRequest request)
    {
        var account = taiKhoanRepository.GetByUsername(request.Username);
        if (account is null || !BCrypt.Net.BCrypt.Verify(request.Password, account.PassWord))
            return ApiResponse<AuthenticatedUser>.Fail("Sai thông tin đăng nhập");

        var profile = userRepository.GetByAccountId(account.ID);
        var role = AppRoles.FromTypeUser(account.TypeUsser);
        return ApiResponse<AuthenticatedUser>.Ok(new AuthenticatedUser(account.ID, profile?.ID, account.Name, role, account.Changepass), "Đăng nhập thành công");
    }

    public ApiResponse<int> Register(RegisterRequest request)
    {
        if (taiKhoanRepository.ExistsUsername(request.Username))
            return ApiResponse<int>.Fail("Tên tài khoản đã tồn tại");

        if (!IsStrongPassword(request.Password))
            return ApiResponse<int>.Fail("Mật khẩu chưa đạt chính sách", "Ít nhất 8 ký tự gồm chữ hoa, chữ thường và số.");

        var account = new TaiKhoan
        {
            Name = request.Username,
            Email = request.Email,
            PassWord = BCrypt.Net.BCrypt.HashPassword(request.Password),
            TypeUsser = request.TypeUser,
            IsAction = true,
            Changepass = true
        };

        taiKhoanRepository.Add(account);
        taiKhoanRepository.SaveChanges();
        return ApiResponse<int>.Ok(account.ID, "Đăng ký thành công, vui lòng đổi mật khẩu ở lần đăng nhập đầu tiên");
    }

    public ApiResponse<bool> ChangePassword(ChangePasswordRequest request)
    {
        var account = taiKhoanRepository.GetById(request.UserId);
        if (account is null)
            return ApiResponse<bool>.Fail("Không tìm thấy tài khoản");

        if (!IsStrongPassword(request.NewPassword))
            return ApiResponse<bool>.Fail("Mật khẩu chưa đạt chính sách");

        account.PassWord = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        account.Changepass = false;
        taiKhoanRepository.Update(account);
        taiKhoanRepository.SaveChanges();
        return ApiResponse<bool>.Ok(true, "Đổi mật khẩu thành công");
    }

    public ApiResponse<bool> ForgotPassword(string email)
    {
        var account = taiKhoanRepository.GetByEmail(email);
        if (account is null)
            return ApiResponse<bool>.Fail("Email chưa được đăng ký");

        var newPass = GenerateRandomPassword();
        account.PassWord = BCrypt.Net.BCrypt.HashPassword(newPass);
        account.Changepass = true;
        taiKhoanRepository.Update(account);
        taiKhoanRepository.SaveChanges();

        var sent = emailService.SendMail(email, "Reset mật khẩu", $"<p>Mật khẩu mới: <b>{newPass}</b></p>");
        if (!sent)
        {
            logger.LogWarning("Reset password updated but email failed for accountId={AccountId}", account.ID);
            return ApiResponse<bool>.Fail("Đã reset mật khẩu nhưng gửi email thất bại");
        }

        return ApiResponse<bool>.Ok(true, "Mật khẩu mới đã gửi qua email");
    }

    private static bool IsStrongPassword(string password) =>
        password.Length >= 8 && password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit);

    private static string GenerateRandomPassword(int length = 10)
    {
        const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
