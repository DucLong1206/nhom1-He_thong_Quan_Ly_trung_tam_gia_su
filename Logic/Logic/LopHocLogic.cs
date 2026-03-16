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

                    if (!string.IsNullOrWhiteSpace(lop.diaChi))
                    {
                        lh.DIaChi = lop.diaChi.Trim();
                    }

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
                else if (lop.loai == "dieuchinhph")
                {
                    // cập nhật trạng thái
                    lh.TrangThai = 9;

                    if (!string.IsNullOrWhiteSpace(lop.diaChi))
                    {
                        lh.DIaChi = lop.diaChi.Trim();
                    }

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
                    lh.TrangThai = 6;
                }
                else if (lop.loai == "dongy")
                {
                    lh.TrangThai = 3;
                }
                else if (lop.loai == "huy")
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
                    PhiMoiGioi = TinhPhiMoiGioi(lophoc, soBuoi),
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
                        var soBuoiCamKet = _context.LopHoc_Buoihocdangki.Count(x => x.IDlophoc == id);
                        var phiMoiGioi = TinhPhiMoiGioi(lop, soBuoiCamKet);
                        lop.PhiMoiGioi = phiMoiGioi;
                        hopdong.PhiMoiGioi = phiMoiGioi;
                        hopdong.SoBuoiCamKet = soBuoiCamKet;
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
        public lophocbyid getlopbyid(int id)
        {
            var data = _context.Set<lophocbyid>()
                .FromSqlRaw("EXEC getlopbyid @id", new SqlParameter("@id", id))
                .AsEnumerable()
                .FirstOrDefault();

            return data;
        }
        public bool savelichbu(Lophoc_doilich model)
        {
            try
            {
                if (model == null)
                {
                    return false;
                }

                _context.Lophoc_doilich.Add(model);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public List<LopHoc_LichHoc> GetNgayGocHocBu(int idlop)
        {
            try
            {
                var data = _context.Set<LopHoc_LichHoc>()
                    .FromSqlRaw("EXEC getngaygochocbu @IDLop",
                        new SqlParameter("@IDLop", idlop))
                    .ToList();

                return data;
            }
            catch (Exception)
            {
                return new List<LopHoc_LichHoc>();
            }
        }
        public bool editlichbu(Lophoc_doilich model)
        {
            try
            {
                var lich = _context.Lophoc_doilich.FirstOrDefault(x => x.ID == model.ID);

                if (lich == null)
                    return false;


                lich.ngaydoi = model.ngaydoi;
                lich.thu = model.thu;
                lich.giobatdau = model.giobatdau;
                lich.gioketthuc = model.gioketthuc;

                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<LichHomNayModel> LichHomNay(int id, int Mode)
        {
            try
            {
                var data = _context.Set<LichHomNayModel>()
                    .FromSqlRaw("EXEC getlichhomnay @UserID, @Mode",
                        new SqlParameter("@UserID", id),
                        new SqlParameter("@Mode", Mode))
                    .ToList();

                return data;
            }
            catch (Exception)
            {
                return new List<LichHomNayModel>();
            }
        }
        public List<ThongTinBuoiHocModel> GetThongTinBuoiHoc(int idlop, int iddk)
        {
            try
            {
                var data = _context.Set<ThongTinBuoiHocModel>()
                    .FromSqlRaw(
                        "EXEC getthongtinbuoihoc @idlop, @iddk",
                        new SqlParameter("@idlop", idlop),
                        new SqlParameter("@iddk", iddk)
                    )
                    .ToList();

                return data;
            }
            catch
            {
                return new List<ThongTinBuoiHocModel>();
            }
        }
        public int luuthongtinbuoihoc(int idlop)
        {
            try
            {
                var param = new SqlParameter("@idlop", idlop);

                var id = _context.Database
                    .SqlQuery<int>($"EXEC luuthongtinbuoihoc @idlop={param}")
                    .AsEnumerable()
                    .FirstOrDefault();

                return id;
            }
            catch
            {
                return 0;
            }
        }
        public LopHoc_BuoiHoc GetTrangThaiBuoiHoc(int ID)
        {
            var data = _context.LopHoc_BuoiHoc.FirstOrDefault(x => x.ID == ID);
            return data;
        }
        public bool StopLesson(int ID)
        {
            var data = _context.LopHoc_BuoiHoc.FirstOrDefault(x => x.ID == ID);

            if (data == null)
                return false;

            data.TrangThai = 2;
            data.GioKetThuc = DateTime.Now.TimeOfDay;

            _context.SaveChanges();

            return true;
        }

        public XuLyHoanPhiResult XuLyHoanPhiKhiLopHong2BuoiDau(int lopId, int idNhanVienXuLy)
        {
            var result = new XuLyHoanPhiResult
            {
                Success = false,
                Message = "Không xử lý được hoàn phí.",
                SoBuoiDaHoc = 0,
                SoTienHoan = 0
            };

            try
            {
                var check = KiemTraDieuKienHoanPhi(lopId);
                result.SoBuoiDaHoc = check.SoBuoiDaHoc;
                result.IDHopDong = check.IDHopDong;

                if (!check.CoTheHoanPhi)
                {
                    result.Message = check.Message;
                    return result;
                }

                var hopDong = _context.HopDong.FirstOrDefault(x => x.ID == check.IDHopDong);
                var lopHoc = _context.LopHoc.FirstOrDefault(x => x.ID == lopId);
                if (hopDong == null || lopHoc == null)
                {
                    result.Message = "Dữ liệu hợp đồng hoặc lớp học không hợp lệ.";
                    return result;
                }

                var soTienHoan = check.SoTienDuKienHoan;

                var hoanPhi = new HoanPhi
                {
                    IDHopDong = hopDong.ID,
                    IDUser = lopHoc.idnguoitao ?? 0,
                    IDnhanvien = 0, // idNhanVienXuLy,
                    SoTienHoan = soTienHoan,
                    NgayXuLy = DateTime.Now
                };

                _context.HoanPhi.Add(hoanPhi);

                hopDong.TrangThai = 3;
                lopHoc.TrangThai = 5;

                _context.SaveChanges();

                result.Success = true;
                result.SoTienHoan = soTienHoan;
                result.Message = $"Xử lý hoàn phí thành công. Số tiền hoàn: {soTienHoan:N0} VNĐ.";
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public KiemTraDieuKienHoanPhiResult KiemTraDieuKienHoanPhi(int lopId)
        {
            var result = new KiemTraDieuKienHoanPhiResult
            {
                CoTheHoanPhi = false,
                Message = "Chưa đủ điều kiện hoàn phí.",
                SoBuoiDaHoc = 0,
                SoTienDuKienHoan = 0
            };

            try
            {
                var hopDong = _context.HopDong.FirstOrDefault(x => x.IDLopHoc == lopId);
                if (hopDong == null)
                {
                    result.Message = "Không tìm thấy hợp đồng của lớp.";
                    return result;
                }

                var lopHoc = _context.LopHoc.FirstOrDefault(x => x.ID == lopId);
                if (lopHoc == null)
                {
                    result.Message = "Không tìm thấy lớp học.";
                    return result;
                }

                var soBuoiDaHoc = _context.LopHoc_BuoiHoc
                    .Count(x => x.IDLop == lopId
                                && (x.TrangThai == 2 || x.GioKetThuc != null));

                result.IDHopDong = hopDong.ID;
                result.SoBuoiDaHoc = soBuoiDaHoc;

                if (_context.HoanPhi.Any(x => x.IDHopDong == hopDong.ID))
                {
                    result.Message = "Hợp đồng này đã có bản ghi hoàn phí trước đó.";
                    return result;
                }

                if (soBuoiDaHoc > 2)
                {
                    result.Message = "Lớp đã quá 2 buổi đầu nên không thuộc diện hoàn phí.";
                    return result;
                }

                var heSoHoan = soBuoiDaHoc <= 1 ? 1.0m : 0.5m;
                result.SoTienDuKienHoan = Math.Round(lopHoc.sotienMotBuoi ?? 0 * heSoHoan, 0, MidpointRounding.AwayFromZero);
                result.CoTheHoanPhi = true;
                result.Message = $"Đủ điều kiện hoàn phí. Số tiền dự kiến hoàn: {result.SoTienDuKienHoan:N0} VNĐ.";

                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        private decimal TinhPhiMoiGioi(LopHoc lop, int soBuoiCamKet)
        {
            var hocPhiMotBuoi = lop.sotienMotBuoi ?? 0;
            var tongHocPhiDuKien = hocPhiMotBuoi * soBuoiCamKet;

            var phiTheoTyLe = tongHocPhiDuKien * 0.15m;

            const decimal phiToiThieu = 20000m;
            const decimal phiToiDa = 500000m;

            var phiSauChanDuoi = Math.Max(phiToiThieu, phiTheoTyLe);
            var phiCuoi = Math.Min(phiSauChanDuoi, phiToiDa);

            return Math.Round(phiCuoi, 0, MidpointRounding.AwayFromZero);
        }
    }
}
