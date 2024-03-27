using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using CRUDExample.Controllers;
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

        public async Task Create_IfNoModelErrors_ToReturnCreateView()
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
            IActionResult result = await peopleController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(personAddRequest);
        }

        #endregion
    }
}
