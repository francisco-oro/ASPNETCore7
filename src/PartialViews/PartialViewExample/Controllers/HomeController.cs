using Microsoft.AspNetCore.Mvc;
using PartialViewExample.Models;

namespace PartialViewExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("programming-languages")]
        public IActionResult ProgrammingLanguages()
        {
            ListModel listModel = new ListModel()
            {
                ListTitle = "Programming Languages list",
                ListItems = new List<string>
                {
                    "Python",
                    "C#",
                    "Go"
                }
            };

            return PartialView("_ListPartialView", listModel);
        }
    }
}
