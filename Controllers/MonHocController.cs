using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.AspNetCore.Mvc;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers;

public class MonHocController : Controller
{
    private readonly IMonhocLogic _mh;

    public MonHocController(IMonhocLogic mh)
    {
        _mh = mh;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToAction("login", "Home");
        }





        return View(vm);
    }
    public JsonResult getlist(string? keyword, int? monHocId, int? xaId, string sort = "name_asc")
    {
        var vm = _mh.GetListGiaSu();

        return Json(new { data = vm });
    }
}
