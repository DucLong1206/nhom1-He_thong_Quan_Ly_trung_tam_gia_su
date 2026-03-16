using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic
{
    public interface IMonhocLogic
    {
        List<danhsanhgiasu_List> GetListGiaSu(string keyword, int idmon, decimal gia, int sapxep);
        List<MonHoc> getlisstmonhoc();
        List<GiaSu_MonHoc_List> getlistmonhocbyidgiasu(int id);
        bool savegiasumonhoc(GiaSu_MonHoc model);
    }
}
