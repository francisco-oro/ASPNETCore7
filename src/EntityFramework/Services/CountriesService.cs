using System.Collections;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        // private field 
        private readonly PeopleDbContext _db;

        //constructor
        public CountriesService(PeopleDbContext peopleDbContext)
        {
            _db = peopleDbContext;
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
            if (_db.Countries.Count(temp => temp.CountryName != null && temp.CountryName.Equals(countryAddRequest.CountryName)) > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type 
            Country country = countryAddRequest.ToCountry();

            //generate CountryID
            country.CountryID = Guid.NewGuid();

            //Add country object into _db
            _db.Add(country);
            _db.SaveChanges();
            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _db.Countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryId)
        {
            if (countryId == null)
            {
                return null;
            }

            Country? countryResponseFromList = _db.Countries.FirstOrDefault(temp => temp.CountryID == countryId);

            if (countryResponseFromList == null)
            {
                return null;
            }

            return countryResponseFromList.ToCountryResponse();
        }
    }
}
