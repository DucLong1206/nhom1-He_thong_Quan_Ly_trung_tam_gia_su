using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.AspNetCore.Mvc;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers
{
    public class USERController : Controller
    {
        private readonly IDM_TinhLogic _t;
        private readonly IDM_XaLogic _x;

        public USERController(IDM_TinhLogic t, IDM_XaLogic x)
        {
            _t = t;
            _x = x;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddorEdit(int idtk)
        {
            var model = new USER
            {
                IDTK = idtk
            };
            return View(model);
        }
        public JsonResult getlistxa(int id)
        {
            var list = _x.GetListbbytinh(id);
            return Json(new { data = list });
        }
        public JsonResult getlisttinh()
        {
            var list = _t.GetList();
            return Json(new { data = list });
        }
        [HttpPost]
        public JsonResult Save(USER model, IFormFile avatarFile)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
            }

            // Validate server nâng cao
            if (model.SDT.Length > 12)
            {
                return Json(new { success = false, message = "SĐT không hợp lệ" });
            }

            // Upload avatar nếu có
            if (avatarFile != null && avatarFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    avatarFile.CopyToAsync(stream);
                }

                model.avata = "/images/" + fileName;
            }

            var user = _user.Save(model);

            return Json(new { success = true, message = "Lưu thành công" });
        }
    }
}
