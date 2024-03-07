﻿using System;
using ServiceContracts.DTO; 
namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity 
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// Adds a new person into the list of people
        /// </summary>
        /// <param name="personAddRequest">Person to add</param>
        /// <returns>Returns the same person details, along with newly generated PersonID</returns>
        PersonResponse AddPerson(PersonAddRequest personAddRequest);
        
        List<PersonResponse> GetAllPeople();
    }
}
