using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic
{
    public interface IUSERLogic
    {
        bool save(USER ur, string mess);
        USER GETBYIDTK(int IDTK);
        bool EDIT(USER ur, out string mess);
        int checkEmailExists(string email, int id);
        bool changepass(string pass, int id, int type);
    }
}
