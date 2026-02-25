using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entities;

[Table("DM_tinh")]
public class DmTinh
{
    [Key]
    public int ID { get; set; }

    [Required, MaxLength(500)]
    public string Name { get; set; } = string.Empty;

    public ICollection<DmXa> DmXas { get; set; } = new List<DmXa>();
}

[Table("DM_XA")]
public class DmXa
{
    [Key]
    public int ID { get; set; }

    [Required, MaxLength(500)]
    public string Name { get; set; } = string.Empty;

    [Column("IDTInh")]
    public int IDTinh { get; set; }

    [ForeignKey(nameof(IDTinh))]
    public DmTinh? DmTinh { get; set; }
}

[Table("MonHoc")]
public class MonHoc
{
    [Key]
    public int ID { get; set; }

    [Required, MaxLength(500)]
    public string Name { get; set; } = string.Empty;
}

[Table("TaiKhoan")]
public class TaiKhoan
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string PassWord { get; set; } = string.Empty;

    public bool TypeUsser { get; set; }

    public int IDuser { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public bool IsAction { get; set; }
}

[Table("NhanVien")]
public class NhanVien
{
    [Key]
    public int ID { get; set; }

    public int IDTK { get; set; }

    [Required, MaxLength(500)]
    public string Name { get; set; } = string.Empty;

    public string DiaChi { get; set; } = string.Empty;

    [MaxLength(12)]
    public string SDT { get; set; } = string.Empty;

    public int IDXa { get; set; }
}

[Table("USER")]
public class User
{
    [Key]
    public int ID { get; set; }

    public int IDTK { get; set; }

    [Required, MaxLength(500)]
    public string Name { get; set; } = string.Empty;

    public string DiaChi { get; set; } = string.Empty;

    [MaxLength(12)]
    public string SDT { get; set; } = string.Empty;

    public int IDXa { get; set; }

    [MaxLength(50)]
    public string STK { get; set; } = string.Empty;

    public int NganHang { get; set; }
}
