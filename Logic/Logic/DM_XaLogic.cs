using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Logic.Logic
{
    public class DM_XaLogic : IDM_XaLogic
    {
        private readonly Appdbcontext _context;
        public DM_XaLogic(Appdbcontext context)
        {
            _context = context;
        }
        public List<DM_XA> GetListbbytinh(int idtinh)
        {
            try
            {

                var result = _context.DM_XA.Where(x => x.IDTInh == idtinh).ToList();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DM_XA GETBYID(int IDXA)
        {
            try
            {
                var XA = _context.DM_XA.FirstOrDefault(X => X.ID == IDXA);
                return XA;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
