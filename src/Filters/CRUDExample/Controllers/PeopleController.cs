﻿using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.ResourceFilters;
using CRUDExample.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Controller", "My-Value-From-Controller", 3 }, Order = 3 )]
    public class PeopleController : Controller
    {
        //private fields
        private readonly IPeopleService _peopleService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PeopleController> _logger;
        // constructor
        public PeopleController(IPeopleService peopleService, ICountriesService countriesService, ILogger<PeopleController> logger)
        {
            _peopleService = peopleService;
            _countriesService = countriesService;
            _logger = logger;
        }

        //Urls: people/index
        [Route("[action]")]
        [Route("/")]
        [TypeFilter(typeof(PeopleListActionFilter), Order = 4)]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-Key-From-Action", "My-Value-From-Action", 1}, Order = 1)]
        [TypeFilter(typeof(PeopleListResultFilter))]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), [FromQuery] SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of PeopleController");

            _logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortBy: {sortBy}");
            //Search
            List<PersonResponse> people = await _peopleService.GetFilteredPeople(searchBy, searchString);
            //Sort
            List<PersonResponse> sortedPeople = await _peopleService.GetSortedPeople(people, sortBy, sortOrder);

            return View(sortedPeople); // Views/People/Index.cshtml
        }

        // Executes when the user clicks on "Create Person" button 
        [Route("[action]")]
        [HttpGet]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "my-key", "my-value", 4})]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse?> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
                new SelectListItem() { Text = temp?.CountryName, Value = temp?.CountryID.ToString() });

            //new SelectListItem() { Text = "Francisco", Value = "1" };
            //<option value="1">Francisco</option>
            return View();
        }

        [HttpPost]
        // Url: People/Create
        [Route("[action]")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(FeatureDisabledResourceFilter), Arguments = new object[] {false })]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            // call the service method
            PersonResponse personResponse = await _peopleService.AddPerson(personRequest);

            // navigate to Index() action method (it makes another get request to "people/index")
            return RedirectToAction("Index", "People");
        }

        [HttpGet]
        [Route("[action]/{personID}")] //Eg: /people/edit/1
        public async Task<IActionResult> Edit(Guid personID)
        {
            PersonResponse? personResponse = await _peopleService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse?> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
                new SelectListItem() { Text = temp?.CountryName, Value = temp?.CountryID.ToString() });

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            PersonResponse? personResponse = await _peopleService.GetPersonByPersonID(personRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            PersonResponse updatedPerson = await _peopleService.UpdatePerson(personRequest);
            return RedirectToAction("Index");

        }

        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse? personResponse = await _peopleService.GetPersonByPersonID(personID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _peopleService.GetPersonByPersonID(personUpdateRequest.PersonID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            await _peopleService.DeletePerson(personUpdateRequest.PersonID);
            return RedirectToAction("Index");
        }

        [Route("[action]")]
        public async Task<IActionResult> PeoplePDF()
        {
            //Get list of people
            List<PersonResponse> people = await _peopleService.GetAllPeople();

            //Return view as pdf
            return new ViewAsPdf("PeoplePDF", people, ViewData)
            {
                PageMargins = new Margins()
                { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Orientation.Landscape
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> PeopleCSV()
        {
            MemoryStream memoryStream = await _peopleService.GetPeopleCSV();
            return File(memoryStream, "application/octet-stream", "people.csv");
        }

        [Route("[action]")]
        public async Task<IActionResult> PeopleExcel()
        {
            MemoryStream memoryStream = await _peopleService.GetPeopleExcel();

            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "people.xlsx");
        }
    }
}
