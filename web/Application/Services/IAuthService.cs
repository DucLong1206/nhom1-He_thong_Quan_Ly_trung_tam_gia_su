using He_thong_Quan_Ly_trung_tam_gia_su.Application.DTOs;
using He_thong_Quan_Ly_trung_tam_gia_su.Models;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Application.Services;

public interface IAuthService
{
    ApiResponse<AuthenticatedUser> Login(LoginRequest request);
    ApiResponse<int> Register(RegisterRequest request);
    ApiResponse<bool> ChangePassword(ChangePasswordRequest request);
    ApiResponse<bool> ForgotPassword(string email);
}
