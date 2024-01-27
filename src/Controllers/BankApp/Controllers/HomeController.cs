using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Content("Welcome to the Best Bank", "text/plain");
        }

        [Route("account-details")]
        public IActionResult AccountDetails()
        {
            return Json(new
            {
                accountNumber = 1001, accountHolderName = "Example Name",
                currentBalance = 5000
            }); 
        }

        [Route("account-statement")]
        public IActionResult AccountStatement()
        {
            return File("Comprobante.pdf", "application/pdf"); 
        }

        [Route("get-current-balance/{accountNumber?}")]
        public IActionResult GetCurrentBalance()
        {
            int accountNumber = Convert.ToInt32(Request.RouteValues["accountNumber"]);
            if (accountNumber  == 1001)
            {
                return Content("5000", "text/plain");
            }
            if (accountNumber == 0)
            {
                return NotFound("Account Number should be supplied");
            }
            return BadRequest("Account Number should be 1001");
        }
    }
}
