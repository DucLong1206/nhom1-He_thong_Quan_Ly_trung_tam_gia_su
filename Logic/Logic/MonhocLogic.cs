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
        public List<MonHoc> getlisstmonhoc()
        {
            var monhoc = _context.MonHoc.ToList();
            return monhoc;
        }
        public List<GiaSu_MonHoc_List> getlistmonhocbyidgiasu(int id)
        {
            var monhoc = _context.GiaSu_MonHoc
                .Where(x => x.IDUser == id)
                .Join(
                    _context.MonHoc,
                    gm => gm.IDMon,
                    m => m.ID,
                    (gm, m) => new GiaSu_MonHoc_List
                    {
                        GiaSu_MonHoc = gm,
                        tenmon = m.Name
                    }
                ).ToList();

            return monhoc;
        }
        public bool savegiasumonhoc(GiaSu_MonHoc model)
        {
            try
            {
                _context.GiaSu_MonHoc.Add(model);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}