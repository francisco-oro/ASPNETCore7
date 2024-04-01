using Microsoft.AspNetCore.Mvc;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        [Route("[action]")]
        [Route("/")]
        [HttpGet]
        public IActionResult Explore()
        {
            return View();
        }
    }
}
