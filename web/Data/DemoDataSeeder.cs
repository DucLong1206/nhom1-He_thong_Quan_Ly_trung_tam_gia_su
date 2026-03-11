using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Data;

public static class DemoDataSeeder
{
    public static void Seed(Appdbcontext db)
    {
        if (!db.TaiKhoan.Any())
        {
            db.TaiKhoan.AddRange(
                new TaiKhoan { Name = "admin", Email = "admin@demo.local", PassWord = BCrypt.Net.BCrypt.HashPassword("Admin123!"), TypeUsser = 0, IsAction = true, Changepass = false },
                new TaiKhoan { Name = "nhanvien", Email = "nv@demo.local", PassWord = BCrypt.Net.BCrypt.HashPassword("Nhanvien123!"), TypeUsser = 1, IsAction = true, Changepass = false },
                new TaiKhoan { Name = "giasu", Email = "giasu@demo.local", PassWord = BCrypt.Net.BCrypt.HashPassword("Giasu123!"), TypeUsser = 2, IsAction = true, Changepass = false },
                new TaiKhoan { Name = "phuhuynh", Email = "ph@demo.local", PassWord = BCrypt.Net.BCrypt.HashPassword("Phuhuynh123!"), TypeUsser = 3, IsAction = true, Changepass = true }
            );
            db.SaveChanges();
        }
    }
}
