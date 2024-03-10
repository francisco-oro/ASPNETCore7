using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        //constructor
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }

        #region AddCountry
        // When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange 
            CountryAddRequest? request = null;

            //Assert 
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act 
                _countriesService.AddCountry(request);
            });
        }
        
        [Fact]
        // When the CountryName is null, it should throw ArgumentException
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange 
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            //Assert 
            Assert.Throws<ArgumentException>(() =>
            {
                // Act 
                _countriesService.AddCountry(request);
            });
        }

        [Fact]
        // When the CountryName is duplicate, it should throw ArgumentException
        public void AddCountry_DuplicateCountryName()
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
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        [Fact]
        // When you supply proper country name, it should insert (add) the country to the existing list of countries
        public void AddCountry_ProperCountryDetails()
        {
            //Arrange 
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            //Act
            CountryResponse response = _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);
        }

        #endregion

        #region GetAllCountries

        [Fact]
        //The list of countries should be empty by default (before adding any countries)
        public void GetAllCountries_EmptyList()
        {
            //Acts 
            List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();

            //Assert 
            Assert.Empty(actual_country_response_list);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            //Arrange 
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName = "USA"},
                new CountryAddRequest() { CountryName = "UK"}
            };

            //Act
            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();
            foreach (CountryAddRequest countryAddRequest in country_request_list)
            {
                countries_list_from_add_country.Add(_countriesService.AddCountry(countryAddRequest));
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            //read each element from countries_list_from_add_country
            foreach (CountryResponse expected_country in countries_list_from_add_country)
            {
                Assert.Contains(expected_country, actualCountryResponseList);
            }
        }

        #endregion

        #region GetCountryByCountryID

        [Fact]
        // If we supply null as countryID, it should return null as CountryResponse 
        public void GetCountryByCountryID_NullCountryID()
        {
            //Arrange 
            Guid? countryID = null;

            //Act 
            CountryResponse countryResponseFromGetMethod =
                _countriesService.GetCountryByCountryID(countryID);
    
            //Assert
            Assert.Null(countryResponseFromGetMethod);
        }


        [Fact]
        // If we supply a valid country id, it should return the matching country details as CountryResponse object
        public void GetCountryID_ValidCountryID()
        {
            //Arrange 
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "China"
            };
            CountryResponse countryResponseFromAddrequest = _countriesService.AddCountry(countryAddRequest);
            
            //Act
            CountryResponse countryResponseFromGet = _countriesService.GetCountryByCountryID(countryResponseFromAddrequest.CountryID);

            //Assert 
            Assert.Equal(countryResponseFromAddrequest,countryResponseFromGet);
        }

        
        #endregion
    }
}

