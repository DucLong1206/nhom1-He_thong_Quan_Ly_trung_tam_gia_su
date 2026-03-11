using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Logic.Logic
{
    public class LopHocLogic : ILopHocLogic
    {
        private readonly Appdbcontext _context;
        public LopHocLogic(Appdbcontext context)
        {
            _context = context;
        }
        public bool SaveLopHoc(SaveLop lh, out string mess)
        {
            mess = "";

            try
            {
                _context.LopHoc.Add(lh.lop);
                _context.SaveChanges();

                //  Gán ID lớp cho từng buổi
                foreach (var buoi in lh.dsBuoi)
                {
                    buoi.ID = null;
                    buoi.IDlophoc = lh.lop.ID;
                }

                //  Thêm danh sách buổi học
                _context.LopHoc_Buoihocdangki.AddRange(lh.dsBuoi);

                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                mess = ex.Message;
                return false;
            }
        }
        public List<LopHoc_List> GetLopHocPing(int id)
        {
            try
            {
                return _context.LopHoc_List
                    .FromSqlRaw("EXEC Lophoc_getlist_ping @id",
                        new SqlParameter("@id", id))
                    .AsNoTracking()
                    .ToList();
            }
            catch (SqlException ex)
            {

                throw new Exception($"Lỗi SQL khi gọi Lophoc_getlist_ping: {ex.Message}", ex);
            }
            catch (Exception ex)
            {

                throw new Exception($"Lỗi hệ thống khi lấy danh sách lớp học: {ex.Message}", ex);
            }
        }
        public List<LopHoc_Buoihocdangki> LopHoc_Buoihocdangki_GetList_ByIDLopHoc(int id)
        {
            try
            {
                List<LopHoc_Buoihocdangki> ds = new List<LopHoc_Buoihocdangki>();
                ds = _context.LopHoc_Buoihocdangki.Where(x => x.IDlophoc == id).ToList();
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ListLop> GETDANHSACHLICHHOC(int id)
        {
            var ds = _context.LopHoc
                .Where(x => x.idnguoinhan == id)
                .Select(x => new ListLop
                {
                    lop = x,

                    nguoiGui = _context.USER
                                .FirstOrDefault(u => u.ID == x.idnguoitao),

                    tenmon = _context.MonHoc
                                .Where(a => a.ID == x.idmon)
                                .Select(a => a.Name)
                                .FirstOrDefault(),

                    dsBuoi = _context.LopHoc_Buoihocdangki
                                .Where(b => b.IDlophoc == x.ID)
                                .ToList()
                })
                .ToList();

            return ds;
        }
        public ListLop GETDANHSACHLICHHOC_byidlophoc(int id)
        {
            var ds = _context.LopHoc
                 .Where(x => x.ID == id)
                 .Select(x => new ListLop
                 {
                     lop = x,

                     nguoiGui = _context.USER
                                 .FirstOrDefault(u => u.ID == x.idnguoitao),

                     tenmon = _context.MonHoc
                                .Where(a => a.ID == x.idmon)
                                 .Select(a => a.Name)
                                 .FirstOrDefault(),

                     dsBuoi = _context.LopHoc_Buoihocdangki
                                 .Where(b => b.IDlophoc == id)
                                 .ToList()
                 })
                 .FirstOrDefault();

            return ds;
        }
        public bool changestatus(ListLopchange lop, out string mess)
        {
            mess = "";

            try
            {
                var lh = _context.LopHoc.FirstOrDefault(x => x.ID == lop.lopid);

                if (lh == null)
                {
                    mess = "Không tìm thấy lớp học";
                    return false;
                }

                if (lop.loai == "dieuchinh")
                {
                    // cập nhật trạng thái
                    lh.TrangThai = 2;

                    // lấy lịch cũ
                    var lichCu = _context.LopHoc_Buoihocdangki
                        .Where(x => x.IDlophoc == lop.lopid)
                        .ToList();

                    // xóa lịch cũ
                    _context.LopHoc_Buoihocdangki.RemoveRange(lichCu);

                    // thêm lịch mới
                    if (lop.dsBuoi != null && lop.dsBuoi.Count > 0)
                    {
                        foreach (var b in lop.dsBuoi)
                        {
                            var buoi = new LopHoc_Buoihocdangki
                            {
                                IDlophoc = lop.lopid,
                                thu = b.thu,
                                giobatdau = b.giobatdau,
                                gioketthuc = b.gioketthuc
                            };

                            _context.LopHoc_Buoihocdangki.Add(buoi);
                        }
                    }
                }

                else if (lop.loai == "tuchoi")
                {
                    lh.TrangThai = 5;
                }

                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                mess = ex.Message;
                return false;
            }
        }
        public HopDong createhopdong(int id)
        {
            var hopdong = _context.HopDong.FirstOrDefault(x => x.IDLopHoc == id);

            if (hopdong == null)
            {
                var lophoc = _context.LopHoc.FirstOrDefault(x => x.ID == id);
                int soBuoi = _context.LopHoc_Buoihocdangki
                      .Count(x => x.IDlophoc == id);
                if (lophoc == null)
                {
                    return null;
                }

                hopdong = new HopDong
                {
                    IDLopHoc = id,
                    NgayBatDau = DateTime.Now,
                    TrangThai = 1,
                    IDGiaSu = lophoc.idnguoinhan,
                    PhiMoiGioi = 20000,
                    SoBuoiCamKet = soBuoi

                };

                _context.HopDong.Add(hopdong);

                _context.SaveChanges();
            }

            return hopdong;
        }
        public bool changeHopDong(int id, int trangthai)
        {
            try
            {
                var hopdong = _context.HopDong.FirstOrDefault(x => x.IDLopHoc == id);
                if (hopdong == null)
                {
                    return false;
                }

                hopdong.TrangThai = trangthai;
                if (trangthai == 2)
                {
                    var lop = _context.LopHoc.FirstOrDefault(x => x.ID == id);
                    if (lop != null)
                    {
                        lop.TrangThai = 3;
                        lop.PhiMoiGioi = 20000;
                    }
                }


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
