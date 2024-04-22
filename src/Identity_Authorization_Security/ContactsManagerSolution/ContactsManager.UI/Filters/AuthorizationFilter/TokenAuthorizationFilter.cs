using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.AuthorizationFilter
{
    public class TokenAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Cookies.ContainsKey("Auth-Key") == false)
            {
                context.Result = new StatusCodeResult(statusCode:StatusCodes.Status401Unauthorized);
                return;
            }

            if (context.HttpContext.Request.Cookies["Auth-Key"] != "A100")
            {
                context.Result = new StatusCodeResult(statusCode: StatusCodes.Status401Unauthorized);
                return;
            }
        }
    }
}
