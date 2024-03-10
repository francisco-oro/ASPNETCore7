using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDExample.Controllers
{
    public class PeopleController : Controller
    {
        //private fields
        private readonly IPersonService _personService;

        // constructor
        public PeopleController(IPersonService personService)
        {
            _personService = personService;
        }

        [Route("people/index")]
        [Route("/")]
        public IActionResult Index()
        {
            List<PersonResponse> people= _personService.GetAllPeople();
            return View(people); // Views/People/Index.cshtml
        }
    }
}
