using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoFixture;
using CRUDExample.Controllers;
using Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDTests
{
    public class PeopleControllerTest
    {
        private readonly IPeopleService _peopleService;
        private readonly ICountriesService _countriesService;

        private readonly Mock<ICountriesService> _countriesServiceMock;
        private readonly Mock<IPeopleService> _peoplesServiceMock;
        
        private readonly Fixture _fixture;

        public PeopleControllerTest()
        {
            _fixture = new Fixture();

            _countriesServiceMock = new Mock<ICountriesService>();
            _peoplesServiceMock = new Mock<IPeopleService>();

            _countriesService = _countriesServiceMock.Object;
            _peopleService = _peoplesServiceMock.Object;
        }

        #region Index
        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPeopleList()
        {
            //Arrange
            List<PersonResponse> personResponsesList = _fixture.Create<List<PersonResponse>>();

            PeopleController peopleController = new PeopleController(_peopleService, _countriesService);

            _peoplesServiceMock
                .Setup(temp => temp.GetFilteredPeople(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(personResponsesList);

            _peoplesServiceMock
                .Setup(temp => temp.GetSortedPeople(It.IsAny<List<PersonResponse>>() , It.IsAny<string>(), It.IsAny<SortOrderOptions>()))
                .ReturnsAsync(personResponsesList);

            //Act 
            IActionResult result = await peopleController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(),_fixture.Create<SortOrderOptions>());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(personResponsesList);
        }

        #endregion

        #region Create

        [Fact]
        public async Task Create_IfModelErrors_ToReturnCreateView()
        {
            //Arrange
            PersonAddRequest personAddRequest = _fixture.Create<PersonAddRequest>();
            PersonResponse personResponse = _fixture.Create<PersonResponse>();

            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>(); 

            PeopleController peopleController = new PeopleController(_peopleService, _countriesService);

            _countriesServiceMock
                .Setup(temp => temp.GetAllCountries())!
                .ReturnsAsync(countries);
            _peoplesServiceMock
                .Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(personResponse);


            //Act 
            peopleController.ModelState.AddModelError("PersonName", "PersonName cannot be blank");

            IActionResult result = await peopleController.Create(personAddRequest);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
            viewResult.ViewData.Model.Should().Be(personAddRequest);
        }

        [Fact]  
        public async Task Create_IfNoModelErrors_ToReturnRedirectToIndex()
        {
            //Arrange
            PersonAddRequest personAddRequest = _fixture.Create<PersonAddRequest>();
            PersonResponse personResponse = _fixture.Create<PersonResponse>();

            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            PeopleController peopleController = new PeopleController(_peopleService, _countriesService);

            _countriesServiceMock
                .Setup(temp => temp.GetAllCountries())!
                .ReturnsAsync(countries);
            _peoplesServiceMock
                .Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(personResponse);


            //Act 

            IActionResult result = await peopleController.Create(personAddRequest);

            //Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ActionName.Should().Be(nameof(PeopleController.Index));
        }

        #endregion

        #region Edit

        [Fact]
        public async Task Edit_IfPersonIdIsNotFound_ToReturnRedirectToIndex()
        {
            //Arrange
            PersonUpdateRequest personUpdateRequest = _fixture.Create<PersonUpdateRequest>();

            _peoplesServiceMock
                .Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(null as PersonResponse);

            PeopleController peopleController = new PeopleController(_peopleService, _countriesService);

            //Act 

            IActionResult result = await peopleController.Edit(personUpdateRequest);

            //Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ActionName.Should().Be(nameof(PeopleController.Index));
        }

        [Fact]
        public async Task Edit_IfModelErrors_ToReturnToEditView()
        {
            //Arrange
            List<Country> countries = new List<Country>()
            {
                _fixture.Build<Country>()
                    .With(temp => temp.People, new List<Person>())
                    .Create(),
                _fixture.Build<Country>()
                    .With(temp => temp.People, new List<Person>())
                    .Create(),
                _fixture.Build<Country>()
                    .With(temp => temp.People, new List<Person>())
                    .Create()
            };

            List<CountryResponse?> countryResponses = countries.Select(temp => temp.ToCountryResponse()).ToList();
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "user@sample.com")
                .With(temp => temp.CountryID, countries[0].CountryID)
                .With(temp => temp.Country, countries[0])
                .With(temp => temp.Gender, "Male")
                .Create();

            PersonUpdateRequest personUpdateRequest = person.ToPersonResponse().ToPersonUpdateRequest();

            
            _countriesServiceMock
                .Setup(temp => temp.GetAllCountries())!
                .ReturnsAsync(countryResponses); 

            _peoplesServiceMock
                .Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(personUpdateRequest.ToPerson().ToPersonResponse);

            PeopleController peopleController = new PeopleController(_peopleService, _countriesService);

            //Act 
            peopleController.ModelState.AddModelError(nameof(PersonUpdateRequest.Email), "Email is invalid");
            
            IActionResult result = await peopleController.Edit(personUpdateRequest);

            //Assert

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonUpdateRequest>();
            viewResult.ViewData.Model.Should().Be(personUpdateRequest);
        }

        [Fact]
        public async Task Edit_IfNoModelErrors_ToReturnRedirectToIndex()
        {
            //Arrange
            PersonUpdateRequest personUpdateRequest = _fixture.Create<PersonUpdateRequest>();

            _peoplesServiceMock
                .Setup(temp => temp.UpdatePerson(It.IsAny<PersonUpdateRequest>()))
                .ReturnsAsync(personUpdateRequest.ToPerson().ToPersonResponse);

            PeopleController peopleController = new PeopleController(_peopleService, _countriesService);

            //Act 

            IActionResult result = await peopleController.Edit(personUpdateRequest);

            //Assert

            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be(nameof(PeopleController.Index));
        }
        #endregion
    }
}
