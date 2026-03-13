using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.AspNetCore.Mvc;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Security;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers;

[SessionAuthorize(AppRoles.PhuHuynhHocVienType)]
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
        return View();
    }
    public JsonResult getlist(string? keyword, int? monHocId, int? xaId, string sort = "name_asc")
    {
        var vm = _mh.GetListGiaSu();

        return Json(new { data = vm });
    }

    public IActionResult TutorDetail(string? tutorName, string? mon, string? xa, decimal? giaTheoGio, int? ID, int? IDXa, int? IDMon, int? Trinhdo, string? avata)
    {
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
