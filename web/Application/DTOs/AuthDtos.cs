using System.ComponentModel.DataAnnotations;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Application.DTOs;

public class LoginRequest
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    [Required, MinLength(4)] public string Username { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(8)] public string Password { get; set; } = string.Empty;
    [Required] public int TypeUser { get; set; }
}

public class ChangePasswordRequest
{
    [Required] public int UserId { get; set; }
    [Required, MinLength(8)] public string NewPassword { get; set; } = string.Empty;
    public bool IsForgotPasswordFlow { get; set; }
}

public record AuthenticatedUser(int AccountId, int? ProfileId, string Username, string Role, bool MustChangePassword);
