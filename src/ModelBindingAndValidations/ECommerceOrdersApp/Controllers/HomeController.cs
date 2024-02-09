using ECommerceOrdersApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrdersApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        [Route("order")]
        public IActionResult Index([FromForm] Order order)
        {
            Random random = new Random();
            order.OrderNo = random.Next(20000);
            if (!ModelState.IsValid)
            {
                string errors = String.Join("\n",
                    ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage).ToList());

                return BadRequest(errors);
            }
            return Json(new { orderNumber = order.OrderNo });
        }
    }
}
