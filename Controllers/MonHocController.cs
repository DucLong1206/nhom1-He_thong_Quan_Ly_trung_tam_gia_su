using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Data;
using He_thong_Quan_Ly_trung_tam_gia_su.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers;

public class MonHocController : Controller
{
    private readonly ApplicationDbContext _db;

    public MonHocController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? keyword, int? monHocId, int? xaId, string sort = "name_asc")
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToAction("login", "Home");
        }

        var query = from gm in _db.GiaSuMonHocs
                    join mh in _db.MonHocs on gm.IDMon equals mh.ID
                    join u in _db.Users on gm.IDUser equals u.ID into userJoin
                    from u in userJoin.DefaultIfEmpty()
                    join gkv in _db.GiaSuKhuVucs on gm.IDUser equals gkv.IDUser into kvJoin
                    from gkv in kvJoin.DefaultIfEmpty()
                    join xa in _db.DmXas on gkv.IDXa equals xa.ID into xaJoin
                    from xa in xaJoin.DefaultIfEmpty()
                    select new MonHocListItemViewModel
                    {
                        MonHocId = mh.ID,
                        TenMon = mh.Name,
                        GiaSuId = gm.IDUser,
                        TenGiaSu = u != null ? u.Name : "(Chưa cập nhật)",
                        GiaTheoGio = gm.GiaTheoGio,
                        XaId = xa != null ? xa.ID : 0,
                        TenXa = xa != null ? xa.Name : "(Chưa cập nhật)"
                    };

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var normalized = keyword.Trim();
            query = query.Where(x => x.TenMon.Contains(normalized) || x.TenGiaSu.Contains(normalized));
        }

        if (monHocId.HasValue)
        {
            query = query.Where(x => x.MonHocId == monHocId.Value);
        }

        if (xaId.HasValue)
        {
            query = query.Where(x => x.XaId == xaId.Value);
        }

        query = sort switch
        {
            "name_desc" => query.OrderByDescending(x => x.TenMon),
            "price_asc" => query.OrderBy(x => x.GiaTheoGio),
            "price_desc" => query.OrderByDescending(x => x.GiaTheoGio),
            _ => query.OrderBy(x => x.TenMon)
        };

        var vm = new MonHocFilterViewModel
        {
            Keyword = keyword,
            MonHocId = monHocId,
            XaId = xaId,
            Sort = sort,
            Items = await query.Take(200).ToListAsync(),
            MonHocOptions = await _db.MonHocs
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Name })
                .ToListAsync(),
            XaOptions = await _db.DmXas
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Name })
                .ToListAsync()
        };

        return View(vm);
    }
}
