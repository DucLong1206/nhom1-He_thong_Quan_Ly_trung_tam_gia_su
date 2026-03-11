using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic
{
    public interface ILopHocLogic
    {
        bool SaveLopHoc(SaveLop lh, out string mess);
        List<LopHoc_List> GetLopHocPing(int id);
        List<LopHoc_Buoihocdangki> LopHoc_Buoihocdangki_GetList_ByIDLopHoc(int id);
        List<ListLop> GETDANHSACHLICHHOC(int id);
        ListLop GETDANHSACHLICHHOC_byidlophoc(int id);
        bool changestatus(ListLopchange lop, out string mess);
        HopDong createhopdong(int id);
        bool changeHopDong(int id, int trangthai);
    }
}
