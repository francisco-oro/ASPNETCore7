using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity 
    /// </summary>
    public interface IPeopleGetterService
    {
        /// <summary>
        /// Returns all people
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        Task<List<PersonResponse>> GetAllPeople();

        /// <summary>
        /// Returns the person object based on the given person id
        /// </summary>
        /// <param name="personID">Person id to search</param>
        /// <returns>Returns matching person object</returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Returns all person objects that match with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Returns all matching persons based on the given search field and search string</returns>
        Task<List<PersonResponse>> GetFilteredPeople(string searchBy, string? searchString);

        /// <summary>
        /// Returns people as CSV
        /// </summary>
        /// <returns>Returns the memory stream as CSV</returns>
        Task<MemoryStream> GetPeopleCSV();

        /// <summary>
        /// Returns people as Excel 
        /// </summary>
        /// <returns>Returns the memory stream with excel data of people</returns>
        Task<MemoryStream> GetPeopleExcel();
    }
}
