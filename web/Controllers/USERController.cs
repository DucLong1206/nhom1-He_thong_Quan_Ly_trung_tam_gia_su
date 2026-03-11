using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su.Application.DTOs;
using He_thong_Quan_Ly_trung_tam_gia_su.Application.Services;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Email;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Security;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers
{
    [Authorize]
    public class USERController : Controller
    {
        private readonly IDM_TinhLogic _t;
        private readonly IDM_XaLogic _x;
        private readonly IUSERLogic _user;
        private readonly IAuthService _authService;

        public USERController(IDM_TinhLogic t, IDM_XaLogic x, IUSERLogic user, IAuthService authService)
        {
            _t = t;
            _x = x;
            _user = user;
            _authService = authService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddorEdit(int idtk)
        {
            ViewBag.idtk = idtk;
            return View();
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
        [ValidateAntiForgeryToken]
        public JsonResult Save(USER model, IFormFile avatarFile)
        {
            string mes = "";


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
            if (model.ID > 0)
            {
                var updateResult = _user.EDIT(model, out mes);
                if (!updateResult)
                    return Json(new { success = false, message = "Cập nhật thất bại" });
                return Json(new { success = true, message = "Cập nhật thành công" });
            }
            else
            {
                var user = _user.save(model, mes);
                return Json(new { success = true, message = "Lưu thành công" });
            }



        }
        [HttpGet]
        public JsonResult GETUSER(int IDTK)
        {
            var ur = _user.GETBYIDTK(IDTK);

            if (ur == null)
                return Json(new { success = false });

            return Json(new { success = true, data = ur });
        }
        public JsonResult GETXABYID(int IDXA)
        {
            var xa = _x.GETBYID(IDXA);
            if (xa == null)
                return Json(new { success = false });
            return Json(new { success = true, data = xa });
        }
        public JsonResult checkpass(string email)
        {
            var result = _authService.ForgotPassword(email);
            return Json(result);
        }
        public JsonResult ChangePass(string pass, int id, int type)
        {
            var result = _authService.ChangePassword(new ChangePasswordRequest
            {
                UserId = id,
                NewPassword = pass,
                IsForgotPasswordFlow = type == 0
            });
            return Json(result);
        }
        public string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
