namespace He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity
{
    public class TaiKhoan
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }
        public bool IsAction { get; set; }
        public bool Changepass { get; set; }
        public int TypeUsser { get; set; }
        public string Email { get; set; }
    }
}
