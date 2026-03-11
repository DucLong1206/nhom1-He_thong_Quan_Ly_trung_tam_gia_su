using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Logic.Logic
{
    public class DM_TinhLogic : IDM_TinhLogic
    {
        private readonly Appdbcontext _context;
        public DM_TinhLogic(Appdbcontext context)
        {
            _context = context;
        }
        public List<DM_tinh> GetList()
        {
            try
            {

                var result = _context.DM_tinh.ToList();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
