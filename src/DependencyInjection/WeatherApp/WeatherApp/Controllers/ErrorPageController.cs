using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.Controllers
{
    [Route("ErrorPage/{statusCode}")]
    public class ErrorPageController : Controller
    {
        public IActionResult Index(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewData["Error"] = "Page Not Found";
                    break;
                default:
                    break;
            }
            return View("ErrorPage");
        }
    }
}
