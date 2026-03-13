using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Security;
namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Appdbcontext _db;
        private readonly ITaiKhoanLogic _tk;

        public HomeController(ILogger<HomeController> logger, Appdbcontext db, ITaiKhoanLogic tk)
        {
            _logger = logger;
            _db = db;
            _tk = tk;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    ViewBag.Error = "Vui lòng nhập đầy đủ tài khoản và mật khẩu.";
                    return View("Login");
                }

                // Hardcode admin
                if (username == "admin" && password == "admin")
                {
                    HttpContext.Session.SetString("IsAdmin", "true");
                    HttpContext.Session.SetInt32("UserId", -1);
                    HttpContext.Session.SetInt32("TypeUsser", 0);
                    HttpContext.Session.SetString("UserName", "admin");

                    return RedirectToAction("Index", "MonHoc");
                }

                // tìm tài khoản
                var user = _db.TaiKhoan.FirstOrDefault(x => x.Name == username);

                if (user == null)
                {
                    ViewBag.Error = "Sai thông tin đăng nhập.";
                    return View("Login");
                }

                bool isPasswordValid = false;

                try
                {
                    // kiểm tra password
                    isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PassWord);
                }
                catch
                {
                    ViewBag.Error = "Mật khẩu hệ thống không hợp lệ.";
                    return View("Login");
                }

                if (!isPasswordValid)
                {
                    ViewBag.Error = "Sai thông tin đăng nhập.";
                    return View("Login");
                }

                // Nếu tài khoản đang ở trạng thái bắt buộc đổi mật khẩu,
                // chuyển thẳng sang màn hình đổi mật khẩu trước khi vào hệ thống.
                if (user.Changepass)
                {
                    return RedirectToAction(nameof(ChangePasswordFirstLogin), new
                    {
                        idtk = user.ID,
                        username = user.Name
                    });
                }

                // lấy user profile
                var ur = _db.USER.FirstOrDefault(x => x.IDTK == user.ID);

                if (ur == null)
                    return RedirectToAction("AddorEdit", "USER", new { idtk = user.ID });

                HttpContext.Session.SetInt32("UserId", ur.ID);
                HttpContext.Session.SetInt32("TypeUsser", user.TypeUsser);
                HttpContext.Session.SetString("UserName", ur?.Name ?? "");

                if (user.TypeUsser == AppRoles.PhuHuynhHocVienType)
                    return RedirectToAction("Index", "MonHoc");

                if (user.TypeUsser == AppRoles.GiaSuType)
                    return RedirectToAction(nameof(TutorDashboard));

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                ViewBag.Error = "Có lỗi xảy ra khi đăng nhập.";
                return View("Login");
            }
        }

        [HttpGet]
        public IActionResult ChangePasswordFirstLogin(int idtk, string username)
        {
            if (idtk <= 0)
                return RedirectToAction(nameof(login));

            ViewBag.IdTk = idtk;
            ViewBag.UserName = username;
            return View();
        }




        [HttpGet]
        [SessionAuthorize(AppRoles.GiaSuType)]
        public IActionResult TutorDashboard()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction(nameof(login));

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DbHealth()
        {
            try
            {
                var canConnect = await _db.Database.CanConnectAsync();

                if (!canConnect)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Không kết nối được DB. Hãy kiểm tra lại ConnectionString/SQL Server."
                    });
                }

                var taiKhoanCount = await _db.TaiKhoan.CountAsync();

                return Json(new
                {
                    success = true,
                    message = "Kết nối DB thành công.",
                    totalTaiKhoan = taiKhoanCount
                });
            }
            catch (DbException dbEx)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi truy cập DB.",
                    detail = dbEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi không xác định khi gọi DB.",
                    detail = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(login));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return Content("Bạn không có quyền truy cập chức năng này.");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Services()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Process()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FAQ()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Policy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(/*new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }*/);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        public JsonResult SaveUser(TaiKhoan tk, string? stypeuser)
        {
            string mes = "";
            if (string.IsNullOrEmpty(tk.Name) || string.IsNullOrEmpty(tk.PassWord))
            {
                return Json(new { success = false, message = "Thiếu thông tin." });
            }

            tk.TypeUsser = string.Equals(stypeuser, "Tutor", StringComparison.OrdinalIgnoreCase)
                ? AppRoles.GiaSuType
                : AppRoles.PhuHuynhHocVienType;

            var check = _tk.save(tk, mes);
            return Json(new { success = check, message = mes });
        }

    }
}
