using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        private readonly IFixture _fixture;
        //constructor
        public CountriesServiceTest()
        {
            _fixture = new Fixture();
            var countriesInitialData = new List<Country>() {  };
            if (countriesInitialData == null) throw new ArgumentNullException(nameof(countriesInitialData));
            
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );

            ApplicationDbContext dbContextOptions = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);

            _countriesService = new CountriesService(dbContextOptions);

        }

        #region AddCountry
        // When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange 
            CountryAddRequest? request = null;

            //Assert 
            await Assert.ThrowsAsync<ArgumentNullException>(async() =>
            {
                // Act 
                await _countriesService.AddCountry(request);
            });
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
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act 
                await _countriesService.AddCountry(request);
            });
        }

        [Fact]
        // When the CountryName is duplicate, it should throw ArgumentException
        public async Task AddCountry_DuplicateCountryName()
        {
            //Arrange 
            CountryAddRequest? request1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest? request2 = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, request1.CountryName)
                .Create();

            //Assert 
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });
        }

        [Fact]
        // When you supply proper country name, it should insert (add) the country to the existing list of countries
        public async Task AddCountry_ProperCountryDetails()
        {
            //Arrange 
            CountryAddRequest? request = _fixture.Create<CountryAddRequest>();
            //Act
            CountryResponse? response = await _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);
        }

        #endregion

        #region GetAllCountries

        [Fact]
        //The list of countries should be empty by default (before adding any countries)
        public async Task GetAllCountries_EmptyList()
        {
            //Acts 
            List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

            //Assert 
            Assert.Empty(actualCountryResponseList);
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Arrange 
            List<CountryAddRequest> countryRequestList = new List<CountryAddRequest>()
            {
                _fixture.Create<CountryAddRequest>(),
                _fixture.Create<CountryAddRequest>()
            };

            //Act
            List<CountryResponse?> countriesListFromAddCountry = new List<CountryResponse?>();
            foreach (CountryAddRequest countryAddRequest in countryRequestList)
            {
                countriesListFromAddCountry.Add(await _countriesService.AddCountry(countryAddRequest));
            }

            List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

            //read each element from countries_list_from_add_country
            foreach (CountryResponse? expectedCountry in countriesListFromAddCountry)
            {
                Assert.Contains(expectedCountry, actualCountryResponseList);
            }
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
            Assert.Null(countryResponseFromGetMethod);
        }


        [Fact]
        // If we supply a valid country id, it should return the matching country details as CountryResponse object
        public async Task GetCountryID_ValidCountryID()
        {
            //Arrange 
            CountryAddRequest? countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse? responseFromAddRequest = await _countriesService.AddCountry(countryAddRequest);
            
            //Act
            CountryResponse? countryResponseFromGet = await _countriesService.GetCountryByCountryID(responseFromAddRequest?.CountryID);

            //Assert 
            Assert.Equal(responseFromAddRequest,countryResponseFromGet);
        }

        
        #endregion
    }
}

