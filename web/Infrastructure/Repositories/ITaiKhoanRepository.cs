using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Repositories;

public interface ITaiKhoanRepository
{
    TaiKhoan? GetByUsername(string username);
    TaiKhoan? GetById(int id);
    TaiKhoan? GetByEmail(string email);
    bool ExistsUsername(string username);
    void Add(TaiKhoan account);
    void Update(TaiKhoan account);
    int SaveChanges();
}
