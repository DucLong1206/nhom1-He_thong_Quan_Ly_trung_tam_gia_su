using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Repositories;

public class TaiKhoanRepository(Appdbcontext db) : ITaiKhoanRepository
{
    public TaiKhoan? GetByUsername(string username) => db.TaiKhoan.FirstOrDefault(x => x.Name == username);
    public TaiKhoan? GetById(int id) => db.TaiKhoan.FirstOrDefault(x => x.ID == id);
    public TaiKhoan? GetByEmail(string email) => db.TaiKhoan.FirstOrDefault(x => x.Email == email);
    public bool ExistsUsername(string username) => db.TaiKhoan.Any(x => x.Name == username);
    public void Add(TaiKhoan account) => db.TaiKhoan.Add(account);
    public void Update(TaiKhoan account) => db.TaiKhoan.Update(account);
    public int SaveChanges() => db.SaveChanges();
}
