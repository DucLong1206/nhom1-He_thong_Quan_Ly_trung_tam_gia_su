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
        1 => NhanVien,
        2 => GiaSu,
        3 => PhuHuynhHocVien,
        _ => PhuHuynhHocVien
    };
}
