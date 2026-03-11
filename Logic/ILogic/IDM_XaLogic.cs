using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic
{
    public interface IDM_XaLogic
    {
        List<DM_XA> GetListbbytinh(int idtinh);
        DM_XA GETBYID(int IDXA);
    }
}
