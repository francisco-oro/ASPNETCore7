using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        //constructor
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(new PeopleDbContext(new DbContextOptionsBuilder<PeopleDbContext>().Options));
            _personService = new PersonService(new PeopleDbContext(
                new DbContextOptionsBuilder<PeopleDbContext>().Options), _countriesService);
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
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

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
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
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
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

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
                new CountryAddRequest() { CountryName = "USA"},
                new CountryAddRequest() { CountryName = "UK"}
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
            CountryResponse countryResponseFromGetMethod =
                await _countriesService.GetCountryByCountryID(countryID);
    
            //Assert
            Assert.Null(countryResponseFromGetMethod);
        }


        [Fact]
        // If we supply a valid country id, it should return the matching country details as CountryResponse object
        public async Task GetCountryID_ValidCountryID()
        {
            //Arrange 
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "China"
            };
            CountryResponse? responseFromAddRequest = await _countriesService.AddCountry(countryAddRequest);
            
            //Act
            CountryResponse? countryResponseFromGet = await _countriesService.GetCountryByCountryID(responseFromAddRequest?.CountryID);

            //Assert 
            Assert.Equal(responseFromAddRequest,countryResponseFromGet);
        }

        
        #endregion
    }
}

