using System;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity 
    /// </summary>
    public interface IPeopleDeleterService
    {
        /// <summary>
        /// Deletes a person based on the given person id
        /// </summary>
        /// <param name="personID">PersonID to delete</param>
        /// <returns>Returns true if the deletion is successful; otherwise false</returns>
        Task<bool> DeletePerson(Guid? personID);
    }
}
