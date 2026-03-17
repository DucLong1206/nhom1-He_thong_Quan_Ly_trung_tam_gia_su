using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity
{
    [Table("ThongBao")]
    public class ThongBao
    {
        [Key]
        public int Id { get; set; }

        // ID của người nhận thông báo (lấy theo Session["UserId"] của bạn)
        public int UserId { get; set; } 

        [Required]
        [MaxLength(500)]
        public string NoiDung { get; set; }

        // Đường dẫn khi người dùng click vào thông báo (VD: "/LopHoc/ChiTiet/1")
        [MaxLength(255)]
        public string Link { get; set; }

        public bool DaDoc { get; set; } = false;

        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}