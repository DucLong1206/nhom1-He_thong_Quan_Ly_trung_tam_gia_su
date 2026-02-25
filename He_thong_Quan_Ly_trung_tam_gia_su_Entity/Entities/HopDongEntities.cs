using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entities;

[Table("HopDong")]
public class HopDong
{
    [Key]
    public int ID { get; set; }

    public int IDLopHoc { get; set; }

    public int IDGiaSu { get; set; }

    public DateTime NgayBatDau { get; set; }

    [Column(TypeName = "decimal(18,0)")]
    public decimal PhiMoiGioi { get; set; }

    public int TrangThai { get; set; }

    public int SoBuoiCamKet { get; set; }
}

[Table("HopDong_LichSu")]
public class HopDongLichSu
{
    [Key]
    public int ID { get; set; }

    public int IDHopDong { get; set; }

    public DateTime NgayThayDoi { get; set; }

    [Column(TypeName = "decimal(18,0)")]
    public decimal PhiMoiGioi { get; set; }

    public int TrangThai { get; set; }

    public int SoBuoiCamKet { get; set; }

    public int IDnguoithaydoi { get; set; }
}

[Table("HoanPhi")]
public class HoanPhi
{
    [Key]
    public int ID { get; set; }

    public int IDHopDong { get; set; }

    [Column(TypeName = "decimal(18,0)")]
    public decimal SoTienHoan { get; set; }

    public DateTime NgayXuLy { get; set; }

    public int IDUser { get; set; }

    public int IDnhanvien { get; set; }
}
