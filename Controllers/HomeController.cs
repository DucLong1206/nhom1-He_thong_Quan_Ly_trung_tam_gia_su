using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Appdbcontext _db;

        public HomeController(ILogger<HomeController> logger, Appdbcontext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ tài khoản và mật khẩu.";
                return View("login");
            }
            if (username == "admin" && password == "admin")
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                HttpContext.Session.SetInt32("AdminId", -1);
                HttpContext.Session.SetString("AdminName", "admin");
            }
            else
            {
                var admin = await _db.TaiKhoan.FirstOrDefaultAsync(x =>
                   x.Name == username &&
                   x.PassWord == password &&
                   x.TypeUsser &&
                   x.IsAction);

                if (admin == null)
                {
                    ViewBag.Error = "Sai thông tin đăng nhập hoặc tài khoản không có quyền Admin.";
                    return View("login");
                }

                HttpContext.Session.SetString("IsAdmin", "true");

            }


            return RedirectToAction("Index", "MonHoc");
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(/*new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }*/);
        }
    }
}
