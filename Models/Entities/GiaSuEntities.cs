using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Models.Entities;

[Table("GiaSu_KhuVuc")]
public class GiaSuKhuVuc
{
    [Key]
    public int ID { get; set; }

    public int IDUser { get; set; }

    public int IDXa { get; set; }
}

[Table("GiaSu_MonHoc")]
public class GiaSuMonHoc
{
    [Key]
    public int ID { get; set; }

    public int IDMon { get; set; }

    [Column(TypeName = "decimal(18,0)")]
    public decimal GiaTheoGio { get; set; }

    public int IDUser { get; set; }
}
