using System.ComponentModel.DataAnnotations;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tài khoản.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng chọn loại người dùng.")]
    [Range(1, 2, ErrorMessage = "Chỉ được chọn Gia sư hoặc Phụ huynh.")]
    public int TypeUser { get; set; }
}

public class CompleteProfileViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
    public string FullName { get; set; } = string.Empty;
}
