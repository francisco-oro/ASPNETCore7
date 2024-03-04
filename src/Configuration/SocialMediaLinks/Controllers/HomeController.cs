using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace SocialMediaLinks.Controllers
{
    public class HomeController : Controller
    {
        private readonly SocialMediaLinksOptions _socialMediaLinksOptions;

        public HomeController(IOptions<SocialMediaLinksOptions> socialMediaLinksOptions)
        {
            _socialMediaLinksOptions = socialMediaLinksOptions.Value; 
        }
        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.Facebook = _socialMediaLinksOptions.Facebook;
            ViewBag.Instagram = _socialMediaLinksOptions.Instagram;
            ViewBag.Instagram = _socialMediaLinksOptions.Instagram;
            ViewBag.Youtube = _socialMediaLinksOptions.Youtube;

            return View();
        }
    }
}
