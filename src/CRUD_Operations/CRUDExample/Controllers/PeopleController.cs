using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

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
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), [FromQuery] SortOrderOptions sortOrder = SortOrderOptions.ASC)
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
            List<PersonResponse> people= _personService.GetFilteredPeople(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString; 
            
            //Sort
            List<PersonResponse> sortedPeople = _personService.GetSortedPeople(people, sortBy, sortOrder);

            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();


            return View(sortedPeople); // Views/People/Index.cshtml
        }
    }
}
