using Microsoft.AspNetCore.Mvc;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly CitiesService _citiesService;
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
