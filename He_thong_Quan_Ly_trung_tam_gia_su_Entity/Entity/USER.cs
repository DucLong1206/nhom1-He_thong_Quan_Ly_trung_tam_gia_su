namespace He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity
{
    public class USER
    {
        public int ID { get; set; }
        public int IDTK { get; set; }
        public string Name { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public int IDXa { get; set; }
        public string STK { get; set; }
        public string? avata { get; set; }
        public int NganHang { get; set; }
    }
    public class danhsanhgiasu_List
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string mon { get; set; }
        public string xa { get; set; }
        public string? avata { get; set; }
        public decimal GiaTheoGio { get; set; }
        public int IDXa { get; set; }
        public int IDMon { get; set; }
        public int Trinhdo { get; set; }
    }
}
