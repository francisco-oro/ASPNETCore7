using System.Collections;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        // private field 
        private readonly List<Country> _countries;

        //constructor
        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.AddRange(new List<Country>()
                {
                    new Country() { CountryID = Guid.Parse("18A3216C-0915-443C-9530-B688390502F7"), CountryName = "USA" },
                    new Country() { CountryID = Guid.Parse("41E5F973-A4A5-4540-9FFD-9716FE8D5CE5"), CountryName = "Canada" },
                    new Country() { CountryID = Guid.Parse("19E5BA7E-F942-4CD9-85F7-6EB16182E165"), CountryName = "UK" },
                    new Country() { CountryID = Guid.Parse("E83B824C-C68D-4AF3-8143-CF2F214F7FCE"), CountryName = "India" },
                    new Country() { CountryID = Guid.Parse("3B3E2AFF-9767-4DB5-A57C-EFAB58F6E9CE"), CountryName = "Australia" },
                });

            }
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            // Validation: countryAddRequest parameter can't be null 
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Validation: countryName can't be null
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            // Validation: CountryName can't be duplicated
            if (_countries.Count(temp => temp.CountryName.Equals(countryAddRequest.CountryName)) > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type 
            Country country = countryAddRequest.ToCountry();

            //generate CountryID
            country.CountryID = Guid.NewGuid();

            //Add country object into _countries
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
            {
                return null;
            }

            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryID);

            if (country_response_from_list == null)
            {
                return null;
            }

            return country_response_from_list.ToCountryResponse();
        }
    }
}
