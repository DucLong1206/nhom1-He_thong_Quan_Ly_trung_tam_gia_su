using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Security;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.AspNetCore.Mvc;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers
{
    public class USERController : Controller
    {
        private readonly IDM_TinhLogic _t;
        private readonly IDM_XaLogic _x;
        private readonly IUSERLogic _user;
        private readonly IMonhocLogic _mh;
        private readonly IDM_NganHangLogic _nh;

        public USERController(IDM_TinhLogic t, IDM_XaLogic x, IUSERLogic user, IMonhocLogic mh, IDM_NganHangLogic nh)
        {
            _t = t;
            _x = x;
            _user = user;
            _mh = mh;
            _nh = nh;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddorEdit(int idtk)
        {
            var guardResult = SessionAccessGuard.EnsureLoggedIn(this);
            if (guardResult != null)
                return guardResult;

            ViewBag.idtk = idtk;
            return View();
        }
        public IActionResult TutorSubjects(int idtk)
        {
            var guardResult = SessionAccessGuard.EnsureUserType(this, 1);
            if (guardResult != null)
                return guardResult;

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
        public JsonResult getlistnganhang()
        {
            var list = _nh.getlist();
            return Json(new { data = list });
        }
        public JsonResult getlisstmonhoc()
        {
            var list = _mh.getlisstmonhoc();
            return Json(new { data = list });
        }
        public JsonResult getlistmonhocbyidgiasu(int id)
        {
            var list = _mh.getlistmonhocbyidgiasu(id);
            return Json(new { data = list });
        }
        [HttpPost]
        public JsonResult SaveMonHocGiaSu(GiaSu_MonHoc model)
        {
            var save = _mh.savegiasumonhoc(model);

            return Json(new { success = save });
        }
        [HttpPost]
        public JsonResult Save(USER model, IFormFile avatarFile)
        {
            var guardResult = SessionAccessGuard.EnsureLoggedIn(this);
            if (guardResult != null)
                return Json(new { success = false, message = "Vui lòng đăng nhập." });

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
            try
            {
                var userId = _user.checkEmailExists(email, 0);

                if (userId <= 0)
                {
                    return Json(new { success = false, message = "Email chưa được đăng ký" });
                }

                // sinh password
                string newPass = GenerateRandomPassword();

                // băm password
                string hashPass = BCrypt.Net.BCrypt.HashPassword(newPass);

                // lưu DB
                var check = _user.changepass(hashPass, userId, 0);

                if (!check)
                {
                    return Json(new { success = false, message = "Reset mật khẩu thất bại" });
                }

                // gửi mail
                EmailService emailService = new EmailService();

                string subject = "Reset mật khẩu hệ thống gia sư";
                string body = $@"
            <h3>Mật khẩu mới của bạn</h3>
            <p>Password: <b>{newPass}</b></p>
            <p>Vui lòng đăng nhập và đổi mật khẩu ngay sau khi đăng nhập.</p>
        ";

                emailService.SendMail(email, subject, body);

                return Json(new { success = true, message = "Mật khẩu mới đã gửi về email của bạn" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public JsonResult ChangePass(string pass, int id, int type)
        {
            try
            {
                // Băm mật khẩu
                string hashPass = BCrypt.Net.BCrypt.HashPassword(pass);

                var check = _user.changepass(hashPass, id, type);

                if (check)
                    return Json(new { success = true, message = "Đổi mật khẩu thành công" });

                return Json(new { success = false, message = "Đổi mật khẩu thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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
