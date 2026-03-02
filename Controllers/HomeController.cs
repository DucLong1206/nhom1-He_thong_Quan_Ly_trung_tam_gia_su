using System.Text;
using He_thong_Quan_Ly_trung_tam_gia_su.Models;
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

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_db.TaiKhoan.Any(x => x.Name == model.Username))
            {
                ModelState.AddModelError(nameof(model.Username), "Tài khoản đã tồn tại.");
                return View(model);
            }

            var account = new TaiKhoan
            {
                Name = model.Username.Trim(),
                PassWord = EncodePassword(model.Password),
                TypeUser = model.TypeUser
            };

            _db.TaiKhoan.Add(account);
            _db.SaveChanges();

            HttpContext.Session.SetInt32("UserAccountId", account.ID);
            HttpContext.Session.SetString("UserName", account.Name ?? string.Empty);

            return RedirectToAction(nameof(CompleteProfile));
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
                var admin = _db.TaiKhoan.FirstOrDefault(x => x.Name == username);

                if (admin == null || !IsPasswordMatch(admin.PassWord, password))
                {
                    ViewBag.Error = "Sai thông tin đăng nhập.";
                    return View("login");
                }

                var ur = _db.USER.FirstOrDefault(x => x.IDTK == admin.ID);
                HttpContext.Session.SetString("IsAdmin", admin.TypeUser == 3 ? "true" : "false");
                HttpContext.Session.SetInt32("UserAccountId", admin.ID);
                HttpContext.Session.SetString("UserName", ur?.Name ?? admin.Name ?? "");

                if (ur == null)
                {
                    return RedirectToAction(nameof(CompleteProfile));
                }

                return RedirectToAction(nameof(Index));

            }

            return RedirectToAction("Index", "MonHoc");
        }

        [HttpGet]
        public IActionResult CompleteProfile()
        {
            var accountId = HttpContext.Session.GetInt32("UserAccountId");
            if (!accountId.HasValue)
            {
                return RedirectToAction(nameof(login));
            }

            var linkedUser = _db.USER.FirstOrDefault(x => x.IDTK == accountId.Value);
            if (linkedUser != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(new CompleteProfileViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompleteProfile(CompleteProfileViewModel model)
        {
            var accountId = HttpContext.Session.GetInt32("UserAccountId");
            if (!accountId.HasValue)
            {
                return RedirectToAction(nameof(login));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var linkedUser = _db.USER.FirstOrDefault(x => x.IDTK == accountId.Value);
            if (linkedUser == null)
            {
                linkedUser = new USER
                {
                    IDTK = accountId.Value,
                    Name = model.FullName.Trim()
                };
                _db.USER.Add(linkedUser);
            }
            else
            {
                linkedUser.Name = model.FullName.Trim();
            }

            _db.SaveChanges();
            HttpContext.Session.SetString("UserName", linkedUser.Name ?? string.Empty);

            return RedirectToAction(nameof(Index));
        }

        private static string EncodePassword(string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }

        private static bool IsPasswordMatch(string? storedPassword, string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(storedPassword))
            {
                return false;
            }

            if (storedPassword == plainPassword)
            {
                return true;
            }

            try
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(storedPassword));
                return decoded == plainPassword;
            }
            catch (FormatException)
            {
                return false;
            }
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
