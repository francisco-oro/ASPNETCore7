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
        public IActionResult Index(string searchBy, string? searchString)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.CountryID), "Country" },
                { nameof(PersonResponse.Address), "Address" },
            };
            List<PersonResponse> people= _personService.GetAllPeople();
            return View(people); // Views/People/Index.cshtml
        }
    }
}
