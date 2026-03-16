namespace He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity
{
    public class LopHoc
    {
        public int ID { get; set; }
        public int? idnguoitao { get; set; }
        public int idnguoinhan { get; set; }
        public decimal? sotienMotBuoi { get; set; }
        public DateTime? ngaytao { get; set; }
        public int? TrangThai { get; set; }
        public bool? isdetele { get; set; }
        public string DIaChi { get; set; }
        public int? IDxa { get; set; }
        public decimal? PhiMoiGioi { get; set; }
        public DateTime NgayBatdau { get; set; }
        public DateTime? NgayKetthuc { get; set; }
        public int? idmon { get; set; }
        public int? trinhdo { get; set; }
    }
    public class LopHoc_List
    {
        public int ID { get; set; }
        public int? idnguoitao { get; set; }
        public int? idnguoinhan { get; set; }
        public decimal? sotienMotBuoi { get; set; }
        public DateTime? ngaytao { get; set; }
        public int? TrangThai { get; set; }
        public bool? isdetele { get; set; }
        public string DIaChi { get; set; }
        public int? IDxa { get; set; }
        public decimal? PhiMoiGioi { get; set; }
        public DateTime NgayBatdau { get; set; }
        public DateTime? NgayKetthuc { get; set; }
        public int? idmon { get; set; }
        public int? trinhdo { get; set; }
        public string? name { get; set; }
        public string? mon { get; set; }
        public string? xa { get; set; }
        public string? trangthainame { get; set; }
    }
    public class SaveLop
    {
        public LopHoc lop { get; set; }
        public List<LopHoc_Buoihocdangki> dsBuoi { get; set; }
    }
    public class ListLop
    {
        public LopHoc lop { get; set; }

        public List<LopHoc_Buoihocdangki> dsBuoi { get; set; }

        public USER nguoiGui { get; set; }

        public string tenmon { get; set; }
    }
    public class ListLopchange
    {
        public int lopid { get; set; }
        public List<LopHoc_Buoihocdangki>? dsBuoi { get; set; }
        public string loai { get; set; }
        public string? diaChi { get; set; }
    }
    public class lophocbyid
    {
        public decimal sotienMotBuoi { get; set; }
        public string mon { get; set; }
        public string Name { get; set; }
        public string trangthai { get; set; }
        public int trinhdo { get; set; }
    }
    public class Lophoc_doilich
    {
        public int ID { get; set; }
        public int idlophoc { get; set; }
        public DateTime ngaydoi { get; set; }
        public DateTime ngaygoc { get; set; }
        public int thu { get; set; }
        public TimeSpan giobatdau { get; set; }
        public TimeSpan gioketthuc { get; set; }
    }
    public class LopHoc_LichHoc
    {
        public int ID { get; set; }
        public DateTime NgayHoc { get; set; }
        public int Thu { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }
    }
    public class LichHomNayModel
    {
        public int ID { get; set; }
        public int iddk { get; set; }
        public int TrangThai { get; set; }
        public DateTime NgayHoc { get; set; }
        public int Thu { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }
        public int TrinhDo { get; set; }
        public string Mon { get; set; }
        public string Name { get; set; }
    }
    public class ThongTinBuoiHocModel
    {
        public int ID { get; set; }
        public int? BuoiHocID { get; set; }
        public DateTime NgayHoc { get; set; }
        public int Thu { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }

        public int trinhdo { get; set; }
        public string mon { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string Giasu { get; set; }
        public string Email_Giasu { get; set; }
        public string DIaChi { get; set; }
    }

    public class XuLyHoanPhiResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? IDHopDong { get; set; }
        public int SoBuoiDaHoc { get; set; }
        public decimal SoTienHoan { get; set; }
    }

    public class KiemTraDieuKienHoanPhiResult
    {
        public bool CoTheHoanPhi { get; set; }
        public string Message { get; set; }
        public int? IDHopDong { get; set; }
        public int SoBuoiDaHoc { get; set; }
        public decimal SoTienDuKienHoan { get; set; }
    }
}
