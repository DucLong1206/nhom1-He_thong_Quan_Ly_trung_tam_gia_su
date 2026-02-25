using He_thong_Quan_Ly_trung_tam_gia_su.Data;
using He_thong_Quan_Ly_trung_tam_gia_su.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
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
                var admin = await _db.TaiKhoans.FirstOrDefaultAsync(x =>
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
                HttpContext.Session.SetInt32("AdminId", admin.ID);
                HttpContext.Session.SetString("AdminName", admin.Name);
            }


            return RedirectToAction("Index", "MonHoc");
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
