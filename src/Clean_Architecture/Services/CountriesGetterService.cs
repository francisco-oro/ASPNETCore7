using System.Collections;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesGetterService : ICountriesGetterService
    {
        // private field 
        private readonly ICountriesRepository _countriesRepository;

        //constructor
        public CountriesGetterService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<List<CountryResponse?>> GetAllCountries()
        {
            return (await _countriesRepository.GetAllCountries())
                .Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryId)
        {
            if (countryId == null)
            {
                return null;
            }

            Country? countryResponseFromList = await _countriesRepository.GetCountryByCountryID(countryId.Value);

            if (countryResponseFromList == null)
            {
                return null;
            }

            return countryResponseFromList.ToCountryResponse();
        }
    }
}
