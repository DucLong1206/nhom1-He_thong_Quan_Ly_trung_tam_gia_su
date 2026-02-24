namespace He_thong_Quan_Ly_trung_tam_gia_su.Models.ViewModels;

public class MonHocListItemViewModel
{
    public int MonHocId { get; set; }
    public string TenMon { get; set; } = string.Empty;
    public int GiaSuId { get; set; }
    public string TenGiaSu { get; set; } = string.Empty;
    public decimal GiaTheoGio { get; set; }
    public int XaId { get; set; }
    public string TenXa { get; set; } = string.Empty;
}
