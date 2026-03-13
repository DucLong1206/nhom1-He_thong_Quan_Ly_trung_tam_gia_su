using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Logic.Logic
{
    public class DM_NganHangLogic : IDM_NganHangLogic
    {
        private readonly Appdbcontext _context;
        public DM_NganHangLogic(Appdbcontext context)
        {
            _context = context;
        }
        public List<DM_NganHang> getlist()
        {
            try
            {
                return _context.DM_NganHang.ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
