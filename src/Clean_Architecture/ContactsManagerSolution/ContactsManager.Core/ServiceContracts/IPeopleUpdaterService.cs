using System;
using ContactsManager.Core.DTO;
using ServiceContracts.Enums;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity 
    /// </summary>
    public interface IPeopleUpdaterService
    {
        /// <summary>
        /// Updates the specified person details based on the given person ID 
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update, including person id</param>
        /// <returns>Returns the person response object after update</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
    }
}
