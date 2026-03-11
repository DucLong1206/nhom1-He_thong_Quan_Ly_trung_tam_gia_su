using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.EntityFrameworkCore;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Logic.Logic
{
    public class MonhocLogic : IMonhocLogic
    {
        private readonly Appdbcontext _context;
        public MonhocLogic(Appdbcontext context)
        {
            _context = context;
        }
        public List<danhsanhgiasu_List> GetListGiaSu()
        {
            try
            {

                var result = _context.Set<danhsanhgiasu_List>()
                            .FromSqlRaw("EXEC [dbo].[GiaSu_listdanhsanhgiasu] ")
                            .ToList();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
