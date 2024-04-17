using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity 
    /// </summary>
    public interface IPeopleSorterService
    {
        /// <summary>
        /// Returns sorted list of people
        /// </summary>
        /// <param name="allPeople">Represents list of people to sort</param>
        /// <param name="sortBy">Name of the property (key), based on which the people should be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns sorted people as PersonResponse list</returns>
        Task<List<PersonResponse>> GetSortedPeople(List<PersonResponse> allPeople, string sortBy, SortOrderOptions sortOrder);
    }
}
