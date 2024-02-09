using IActionResultExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("bookstore/{bookid?}/{isloggedin?}")]
        public IActionResult Index(int? bookid,[FromRoute] bool? isloggedin, Book book)
        {
            // Book id should be applied 
            if (bookid.HasValue == false)
            {
                /*return Content("Book id is not supplied");*/
                return BadRequest("Book id is not supplied");
            }
            // Book id can't be empty
            if (bookid <= 0)
            {
                return BadRequest("Book id can't be null or empty");
            }

            if (bookid > 1000)
            {
                return NotFound("Book id can't be greater than 1000");
            }

            // isloggedin should be true
            if (isloggedin == false)
            {
                /*return Unauthorized("User must be authenticated");*/
                return StatusCode(401);
            }

            return Content($"Book id: {book}, Book: {book}", "text/plain");
            /*return new RedirectToActionResult("Books", "Store", new { });*/ // 302 - found
            // return RedirectToAction("Books", "Store", new { Id = bookId });
            // return new RedirectToActionResult("Books", "Store", new { }, permanent: true); // 301 - Moved permanently
/*            return RedirectToActionPermanent("Books", "Store", new { Id = bookId });

            return new LocalRedirectResult($"store/books/{bookId}", true);  // 301 - Moved permanently
            return LocalRedirect($"store/books/{bookId}"); // 302 - Found

            return Redirect($"store/books/{bookId}"); // 302 - Found*/
            /*return RedirectPermanent($"store/books/{bookid}");*/

        }
    }
}
