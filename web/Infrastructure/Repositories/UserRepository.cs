using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Repositories;

public class UserRepository(Appdbcontext db) : IUserRepository
{
    public USER? GetByAccountId(int accountId) => db.USER.FirstOrDefault(x => x.IDTK == accountId);
}
