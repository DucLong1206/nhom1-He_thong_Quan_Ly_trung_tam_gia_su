namespace He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Security;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string NhanVien = "NhanVien";
    public const string GiaSu = "GiaSu";
    public const string PhuHuynhHocVien = "PhuHuynhHocVien";

    public static string FromTypeUser(int typeUser) => typeUser switch
    {
        0 => Admin,
        1 => GiaSu,
        2 => PhuHuynhHocVien,
        3 => NhanVien,
        _ => PhuHuynhHocVien
    };

    public const int GiaSuType = 1;
    public const int PhuHuynhHocVienType = 2;
}
