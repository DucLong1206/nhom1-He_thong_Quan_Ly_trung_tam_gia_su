using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly int[] _allowedTypes;

    public SessionAuthorizeAttribute(params int[] allowedTypes)
    {
        _allowedTypes = allowedTypes ?? Array.Empty<int>();
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var session = context.HttpContext.Session;
        var isAdmin = string.Equals(session.GetString("IsAdmin"), "true", StringComparison.OrdinalIgnoreCase);

        if (!isAdmin && session.GetInt32("UserId") == null)
        {
            context.Result = new RedirectToActionResult("login", "Home", null);
            return;
        }

        if (_allowedTypes.Length == 0 || isAdmin)
            return;

        var typeUser = session.GetInt32("TypeUsser");
        if (typeUser == null || !_allowedTypes.Contains(typeUser.Value))
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
        }
    }
}
