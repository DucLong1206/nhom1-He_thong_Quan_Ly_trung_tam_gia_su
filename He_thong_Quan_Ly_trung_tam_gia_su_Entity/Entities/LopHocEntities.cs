using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entities;

[Table("LopHoc")]
public class LopHoc
{
    [Key]
    public int ID { get; set; }

    public int idnguoitao { get; set; }

    public int idnguoinhan { get; set; }

    public int sobuoi { get; set; }

    [Column(TypeName = "decimal(18,0)")]
    public decimal sotienMotBuoi { get; set; }

    public DateTime ngaytao { get; set; }

    public int trangthai { get; set; }

    public int isdetele { get; set; }

    public string DIaChi { get; set; } = string.Empty;

    public int IDxa { get; set; }

    public double PhiMoiGioi { get; set; }
}

[Table("LopHoc_BuoiHoc")]
public class LopHocBuoiHoc
{
    [Key]
    public int ID { get; set; }

    public int IDLop { get; set; }

    public int BuoiSo { get; set; }

    public DateTime NgayHoc { get; set; }

    public TimeSpan GioBatDau { get; set; }

    public TimeSpan GioKetThuc { get; set; }

    public int TrangThai { get; set; }

    public string Lydo { get; set; } = string.Empty;
}

[Table("LopHoc_LichSu")]
public class LopHocLichSu
{
    [Key]
    public int ID { get; set; }

    public int sobuoi { get; set; }

    [Column(TypeName = "decimal(18,0)")]
    public decimal sotienMotBuoi { get; set; }

    public DateTime ngayThayDoi { get; set; }

    public int trangthai { get; set; }

    public string DIaChi { get; set; } = string.Empty;

    public int IDxa { get; set; }

    [Column(TypeName = "decimal(18,0)")]
    public decimal PhiMoiGioi { get; set; }

    public int IDNGuoiThayDoi { get; set; }

    public int IDLop { get; set; }
}
