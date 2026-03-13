using Microsoft.AspNetCore.Mvc;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Security;

public static class SessionAccessGuard
{
    public static IActionResult? EnsureLoggedIn(Controller controller)
    {
        if (controller.HttpContext.Session.GetInt32("UserId") != null)
        {
            return null;
        }

        return controller.RedirectToAction("login", "Home");
    }

    public static IActionResult? EnsureUserType(Controller controller, int requiredType)
    {
        var loginResult = EnsureLoggedIn(controller);
        if (loginResult != null)
        {
            return loginResult;
        }

        var typeUsser = controller.HttpContext.Session.GetInt32("TypeUsser");
        if (typeUsser == requiredType)
        {
            return null;
        }

        controller.TempData["AccessDeniedMessage"] = "Bạn không có quyền truy cập vào khu vực này.";

        if (typeUsser == 1)
        {
            return controller.RedirectToAction("TutorDashboard", "Home");
        }

        if (typeUsser == 2)
        {
            return controller.RedirectToAction("Index", "MonHoc");
        }

        return controller.RedirectToAction("Index", "Home");
    }
}
