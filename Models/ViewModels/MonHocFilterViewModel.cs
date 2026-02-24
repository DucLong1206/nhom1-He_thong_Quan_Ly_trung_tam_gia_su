using Microsoft.AspNetCore.Mvc.Rendering;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Models.ViewModels;

public class MonHocFilterViewModel
{
    public string? Keyword { get; set; }
    public int? MonHocId { get; set; }
    public int? XaId { get; set; }
    public string Sort { get; set; } = "name_asc";

    public List<SelectListItem> MonHocOptions { get; set; } = new();
    public List<SelectListItem> XaOptions { get; set; } = new();
    public List<MonHocListItemViewModel> Items { get; set; } = new();
}
