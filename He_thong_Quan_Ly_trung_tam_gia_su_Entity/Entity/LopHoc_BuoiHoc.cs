namespace He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity
{
    public class LopHoc_BuoiHoc
    {
        public int ID { get; set; }
        public int IDLop { get; set; }
        public int BuoiSo { get; set; }
        public DateTime NgayHoc { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }
        public int TrangThai { get; set; }
        public string? Lydo { get; set; }
    }
}
