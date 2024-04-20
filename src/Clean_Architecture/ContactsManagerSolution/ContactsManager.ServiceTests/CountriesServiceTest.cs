using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesGetterService _countriesService;

        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
        private readonly ICountriesRepository _countriesRepository;

        private readonly IFixture _fixture;
        //constructor
        public CountriesServiceTest()
        {
            _fixture = new Fixture();
            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _countriesRepository = _countriesRepositoryMock.Object;

            var countriesInitialData = new List<Country>() {  };
            if (countriesInitialData == null) throw new ArgumentNullException(nameof(countriesInitialData));

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            
            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);

            _countriesService = new CountriesUploaderService(_countriesRepository);
        }

        #region AddCountry
        // When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry_ToBeArgumentNullException()
        {
            //Arrange 
            CountryAddRequest? request = null;

            //Assert 
            Func<Task> action = async () =>
            {
                // Act 
                await _countriesService.AddCountry(request);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        
        [Fact]
        // When the CountryName is null, it should throw ArgumentException
        public async Task AddCountry_CountryNameIsNull()
        {
            //Arrange 
            CountryAddRequest? request = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, null as string)
                .Create();

            //Assert 
            Func<Task> Action = async () =>
            {
                // Act 
                await _countriesService.AddCountry(request);
            };
            await Action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        // When the CountryName is duplicate, it should throw ArgumentException
        public async Task AddCountry_DuplicateCountryName_ToBeArgumentException()
        {
            //Arrange 
            CountryAddRequest? request1 = _fixture.Create<CountryAddRequest>();

            _countriesRepositoryMock
                .Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
                .ReturnsAsync(request1.ToCountry);
            //Assert 
            Func<Task> action = async () =>
            {
                // Act
                await _countriesService.AddCountry(request1);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        // When you supply proper country name, it should insert (add) the country to the existing list of countries
        public async Task AddCountry_ProperCountryDetails_ShouldBeSuccessful()
        {
            //Arrange 
            CountryAddRequest? request = _fixture.Create<CountryAddRequest>();
            Country country = request.ToCountry();
            _countriesRepositoryMock
                .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(country);
            //Act
            CountryResponse? response = await _countriesService.AddCountry(request);
            country.CountryID = response.CountryID;
            //Assert
            response.Should().Be(country.ToCountryResponse());
        }

        #endregion

        #region GetAllCountries

        [Fact]
        //The list of countries should be empty by default (before adding any countries)
        public async Task GetAllCountries_EmptyList()
        {
            //Arrange
            _countriesRepositoryMock.Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(new List<Country>());

            //Acts 
            List<CountryResponse?> actualCountryResponseList = await _countriesService.GetAllCountries();

            //Assert 
            actualCountryResponseList.Should().BeEmpty(); 
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Arrange 
            List<Country> countries = new List<Country>()
            {
                _fixture.Build<Country>()
                    .With(temp => temp.People, null as List<Person>)
                    .Create(),
                _fixture.Build<Country>()
                    .With(temp => temp.People, null as List<Person>)
                    .Create(),
            };

            List<CountryResponse?> countryResponsesExpected =
                countries.Select(temp => temp.ToCountryResponse()).ToList();

            _countriesRepositoryMock
                .Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);
            //Act

            List<CountryResponse?> actualCountryResponseList = await _countriesService.GetAllCountries();

            //read each element from countries_list_from_add_country
            actualCountryResponseList.Should().BeEquivalentTo(countryResponsesExpected); 
        }

        #endregion

        #region GetCountryByCountryID

        [Fact]
        // If we supply null as countryID, it should return null as CountryResponse 
        public async Task GetCountryByCountryID_NullCountryID()
        {
            //Arrange 
            Guid? countryID = null;

            //Act 
            CountryResponse? countryResponseFromGetMethod =
                await _countriesService.GetCountryByCountryID(countryID);
    
            //Assert
            countryResponseFromGetMethod.Should().BeNull();
        }


        [Fact]
        // If we supply a valid country id, it should return the matching country details as CountryResponse object
        public async Task GetCountryID_ValidCountryID()
        {
            //Arrange 
            Country country = _fixture.Build<Country>()
                .With(temp => temp.People, null as List<Person>)
                .Create();
            _countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryID(It.IsAny<Guid>()))
                .ReturnsAsync(country);
            //Act
            CountryResponse? countryResponseFromGet = await _countriesService.GetCountryByCountryID(country.CountryID);

            //Assert 
            countryResponseFromGet.Should().Be(country.ToCountryResponse());
        }

        
        #endregion
    }
}

