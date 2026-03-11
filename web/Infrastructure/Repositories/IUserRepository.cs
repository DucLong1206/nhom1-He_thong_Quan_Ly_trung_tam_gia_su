using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Repositories;

public interface IUserRepository
{
    USER? GetByAccountId(int accountId);
}
