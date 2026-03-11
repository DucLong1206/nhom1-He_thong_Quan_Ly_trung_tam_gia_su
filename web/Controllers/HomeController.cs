using He_thong_Quan_Ly_trung_tam_gia_su.Application.DTOs;
using He_thong_Quan_Ly_trung_tam_gia_su.Application.Services;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Security;
using He_thong_Quan_Ly_trung_tam_gia_su.Models;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Security.Claims;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers;

public class HomeController(ILogger<HomeController> logger, Appdbcontext db, IAuthService authService) : Controller
{
    public IActionResult Index() => View();
    public IActionResult Login() => View();
    [HttpGet] public IActionResult ForgotPassword() => View();
    [HttpGet] public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid) return View(request);

        var result = authService.Login(request);
        if (!result.Success || result.Data is null)
        {
            ViewBag.Error = result.Message;
            return View();
        }

        var user = result.Data;
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role)
        };
        if (user.ProfileId.HasValue) claims.Add(new Claim("ProfileId", user.ProfileId.Value.ToString()));

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

        if (user.MustChangePassword)
            return RedirectToAction(nameof(ChangePasswordFirstLogin), new { idtk = user.AccountId, username = user.Username });

        return user.Role switch
        {
            AppRoles.Admin or AppRoles.NhanVien => RedirectToAction("Index", "MonHoc"),
            AppRoles.GiaSu => RedirectToAction(nameof(TutorDashboard)),
            _ => RedirectToAction("Index", "LopHoc")
        };
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SaveUser(RegisterRequest request)
    {
        if (!ModelState.IsValid) return Json(ApiResponse<int>.Fail("Dữ liệu không hợp lệ"));
        var result = authService.Register(request);
        return Json(result);
    }

    [Authorize]
    [HttpGet]
    public IActionResult ChangePasswordFirstLogin(int idtk, string username)
    {
        ViewBag.IdTk = idtk;
        ViewBag.UserName = username;
        return View();
    }

    [Authorize(Roles = AppRoles.GiaSu)]
    [HttpGet]
    public IActionResult TutorDashboard() => View();

    [HttpGet]
    public async Task<IActionResult> DbHealth()
    {
        try
        {
            var canConnect = await db.Database.CanConnectAsync();
            if (!canConnect) return Json(ApiResponse<object>.Fail("Không kết nối được DB"));
            var taiKhoanCount = await db.TaiKhoan.CountAsync();
            return Json(ApiResponse<object>.Ok(new { totalTaiKhoan = taiKhoanCount }, "Kết nối DB thành công"));
        }
        catch (DbException ex) { return StatusCode(500, ApiResponse<object>.Fail("Lỗi truy cập DB", ex.Message)); }
        catch (Exception ex) { return StatusCode(500, ApiResponse<object>.Fail("Lỗi không xác định", ex.Message)); }
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    public IActionResult Privacy() => View();
    [HttpGet] public IActionResult Services() => View();
    [HttpGet] public IActionResult Process() => View();
    [HttpGet] public IActionResult FAQ() => View();
    [HttpGet] public IActionResult Contact() => View();
    [HttpGet] public IActionResult Policy() => View();
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}
