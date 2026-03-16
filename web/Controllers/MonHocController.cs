using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Security;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.AspNetCore.Mvc;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers;

public class MonHocController : Controller
{
    private readonly IMonhocLogic _mh;
    private readonly ILopHocLogic _lh;

    public MonHocController(IMonhocLogic mh, ILopHocLogic lh)
    {
        _mh = mh;
        _lh = lh;
    }

    public IActionResult Index()
    {
        var guardResult = SessionAccessGuard.EnsureUserType(this, 2);
        if (guardResult != null)
            return guardResult;

        return View();
    }
    public JsonResult getlist(string keyword, int idmon, decimal gia, int sapxep)
    {
        var guardResult = SessionAccessGuard.EnsureUserType(this, 2);
        if (guardResult != null)
            return Json(new { success = false, message = "Bạn không có quyền truy cập vào khu vực này." });

        var vm = _mh.GetListGiaSu(keyword, idmon, gia, sapxep);

        return Json(new { data = vm });
    }

    public IActionResult TutorDetail(string? tutorName, string? mon, string? xa, decimal? giaTheoGio, int? ID, int? IDXa, int? IDMon, int? Trinhdo, string? avata)
    {
        var guardResult = SessionAccessGuard.EnsureUserType(this, 2);
        if (guardResult != null)
            return guardResult;

        ViewData["TutorName"] = tutorName;
        ViewData["Mon"] = mon;
        ViewData["Xa"] = xa;
        ViewData["GiaTheoGio"] = giaTheoGio;
        ViewData["ID"] = ID;
        ViewData["IDXa"] = IDXa;
        ViewData["IDMon"] = IDMon;
        ViewData["avata"] = avata;
        ViewData["Trinhdo"] = Trinhdo;

        return View();
    }

    [HttpPost]
    public JsonResult Save([FromBody] SaveLop model)
    {
        var guardResult = SessionAccessGuard.EnsureUserType(this, 2);
        if (guardResult != null)
            return Json(new { success = false, message = "Bạn không có quyền truy cập vào khu vực này." });

        string mess = "";
        model.lop.ngaytao = DateTime.Now;
        model.lop.idnguoitao = HttpContext.Session.GetInt32("UserId");
        model.lop.isdetele = false;
        model.lop.TrangThai = 1;

        var save = _lh.SaveLopHoc(model, out mess);

        if (mess.Length > 0) return Json(new { success = false, mess = mess });
        return Json(new { success = true });
    }
}
