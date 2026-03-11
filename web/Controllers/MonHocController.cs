using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Security;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers;

[Authorize(Roles = AppRoles.Admin + "," + AppRoles.NhanVien + "," + AppRoles.PhuHuynhHocVien)]
public class MonHocController : Controller
{
    private readonly IMonhocLogic _mh;
    private readonly ILopHocLogic _lh;

    public MonHocController(IMonhocLogic mh, ILopHocLogic lh)
    {
        _mh = mh;
        _lh = lh;
    }

    public IActionResult Index() => View();
    public JsonResult getlist(string? keyword, int? monHocId, int? xaId, string sort = "name_asc")
    {
        var vm = _mh.GetListGiaSu();

        return Json(new { data = vm });
    }

    public IActionResult TutorDetail(string? tutorName, string? mon, string? xa, decimal? giaTheoGio, int? ID, int? IDXa, int? IDMon, int? Trinhdo)
    {
        ViewData["TutorName"] = tutorName;
        ViewData["Mon"] = mon;
        ViewData["Xa"] = xa;
        ViewData["GiaTheoGio"] = giaTheoGio;
        ViewData["ID"] = ID;
        ViewData["IDXa"] = IDXa;
        ViewData["IDMon"] = IDMon;
        ViewData["Trinhdo"] = Trinhdo;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public JsonResult Save([FromBody] SaveLop model)
    {
        string mess = "";
        model.lop.ngaytao = DateTime.Now;
        var profileClaim = User.FindFirstValue("ProfileId");
        model.lop.idnguoitao = int.TryParse(profileClaim, out var id) ? id : null;
        model.lop.isdetele = false;
        model.lop.TrangThai = 1;

        var save = _lh.SaveLopHoc(model, out mess);

        if (mess.Length > 0) return Json(new { success = false, mess = mess });
        return Json(new { success = true });
    }
}
