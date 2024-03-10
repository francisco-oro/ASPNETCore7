using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers
{
    public class PeopleController : Controller
    {
        [Route("people/index")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
