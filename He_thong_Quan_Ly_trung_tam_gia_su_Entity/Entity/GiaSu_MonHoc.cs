namespace He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity
{
    public class GiaSu_MonHoc
    {
        public int ID { get; set; }
        public int IDUser { get; set; }
        public decimal GiaTheoGio { get; set; }
        public int IDMon { get; set; }
        public int Trinhdo { get; set; }
        public bool? Isdelete { get; set; }
    }
    public class GiaSu_MonHoc_List
    {

        public GiaSu_MonHoc GiaSu_MonHoc { get; set; }
        public string tenmon { get; set; }

    }
}
