namespace He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity
{
    public class BinhLuan
    {
        public int ID { get; set; }
        public int IDGiaSu { get; set; } // Người dươc bình luận
        public int IDPhuHuynh { get; set; } // Người viết bình luận
        public int idbinhluan { get; set; } // Người viết bình luận
        public string NoiDung { get; set; }
        public DateTime NgayTao { get; set; }
    }
}