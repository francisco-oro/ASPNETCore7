﻿using ServiceContracts.DTO;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest">Country object to add</param>
        /// <returns>Returns the country object after adding it (including newly generated country id)</returns>
        Task<CountryResponse?> AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns all countries from the list
        /// </summary>
        /// <returns>All countries from the list as a List of CountryResponse</returns>
        Task<List<CountryResponse?>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the given country id
        /// </summary>
        /// <param name="countryID">CountryID (guid)to search)</param>
        /// <returns>Matching country as CountryResponse object</returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);

        /// <summary>
        /// Uploads countries from excel file into database
        /// </summary>
        /// <param name="formFile">Returns number of countries added</param>
        /// <returns></returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
