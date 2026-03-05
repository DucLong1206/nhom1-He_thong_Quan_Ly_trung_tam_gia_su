using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.AspNetCore.Mvc;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers
{
    public class LopHocController : Controller
    {
        private readonly ILopHocLogic _lh;
        public LopHocController(ILopHocLogic lh)
        {
            _lh = lh;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Getlistlophocdangkiping(int id)
        {
            try
            {
                var data = _lh.GetLopHocPing(id);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public JsonResult LopHoc_Buoihocdangki_GetList_ByIDLopHoc(int id)
        {
            try
            {
                var data = _lh.LopHoc_Buoihocdangki_GetList_ByIDLopHoc(id);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
