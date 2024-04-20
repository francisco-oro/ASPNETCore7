using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContactsManager.Infrastructure.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PeopleRepository> _logger;

        public PeopleRepository(ApplicationDbContext db, ILogger<PeopleRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Person> AddPerson(Person person)
        {
            await _db.People.AddAsync(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task<List<Person>> GetAllPeople()
        {
            _logger.LogInformation("GetAllPeople of PeopleRepository");
            return await _db.People.Include("Country").ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonID(Guid personID)
        {
            return await _db.People.Include("Country")
                .FirstOrDefaultAsync(temp => temp.PersonID == personID);
        }

        public async Task<List<Person>> GetFilteredPeople(Expression<Func<Person, bool>> predicate)
        {
            _logger.LogInformation("GetFilteredPeople in PeopleRepository");  
            return await _db.People.Include("Country")
                .Where(predicate)
                .ToListAsync();

        }

        public async Task<bool> DeletePersonByPersonID(Guid personID)
        {
            _db.People.RemoveRange(_db.People.Where(temp => temp.PersonID == personID));
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<Person?> UpdatePerson(Person person)
        {
            Person? matchingPerson = await _db.People.FirstOrDefaultAsync(temp => temp.PersonID == person.PersonID);

            if (matchingPerson == null)
            {
                return matchingPerson;
            }

            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Email = person.Email;
            matchingPerson.Gender = person.Gender;
            matchingPerson.CountryID = person.CountryID;
            matchingPerson.Address = person.Address;
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

            int countUpdated = await _db.SaveChangesAsync();

            return matchingPerson; 
        }
    }
}
