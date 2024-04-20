using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StocksApp.UI.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [Route("[action]")]
        public IActionResult Error()
        {
            IExceptionHandlerFeature? exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature != null)
            {
                ViewBag.ErrorMessage = exceptionFeature.Error.Message;
            }
            return View();
        }
    }
}
