﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ContactsManager.Core.Domain.Entities;

namespace ContactsManager.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Person entity
    /// </summary>
    public interface IPeopleRepository
    {
        /// <summary>
        /// Adds a person object to the data store
        /// </summary>
        /// <param name="person">Person object to add</param>
        /// <returns>Returns the person object after adding it to the table</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Returns all people in the data store
        /// </summary>
        /// <returns>List of person objects from table</returns>
        Task<List<Person>> GetAllPeople();

        /// <summary>
        /// Returns a person object based on the given person id
        /// </summary>
        /// <param name="personID">PersonID (guid) to search</param>
        /// <returns>A person object or null</returns>
        Task<Person?> GetPersonByPersonID(Guid personID);

        /// <summary>
        /// Returns all person objects based on the given expression 
        /// </summary>
        /// <param name="predicate">LINQ expression to check</param>
        /// <returns>All matching people with given condition</returns>
        Task<List<Person>> GetFilteredPeople(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Deletes a person object based on the person id
        /// </summary>
        /// <param name="personID">Person ID (guid) to search</param>
        /// <returns>Returns true, if the deletion is successful; otherwise false</returns>
        Task<bool> DeletePersonByPersonID(Guid personID);

        /// <summary>
        /// Updates a person object (person name and other details) based on the given person id
        /// </summary>
        /// <param name="person">Person object to update</param>
        /// <returns>Returns the updated person object</returns>
        Task<Person?> UpdatePerson(Person person);
    }
}