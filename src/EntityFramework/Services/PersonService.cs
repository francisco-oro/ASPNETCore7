
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CsvHelper;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        //private fields
        private readonly PeopleDbContext _db;
        private readonly ICountriesService _countriesService;
        
        //constructor 
        public PersonService(PeopleDbContext db, ICountriesService countriesService)
        {
            _db = db;
            _countriesService = countriesService;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if PersonAddRequest is not null
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model validations
            ValidationHelper.ModelValidation(personAddRequest);

            // convert personAddRequest into Person type 
            Person person = personAddRequest.ToPerson();

            //generate personID
            person.PersonID = Guid.NewGuid();

            //add person object to people list
            _db.People.Add(person);
            await _db.SaveChangesAsync();
            //_db.sp_InsertPerson(person);
            // Convert the Person object into PersonResponse type
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetAllPeople()
        {
            // SELECT * from People
            var people =await  _db.People.Include("Country").ToListAsync();

            return people
                .Select(temp => temp.ToPersonResponse()).ToList();

            //return _db.sp_GetAllPeople()
            //    .Select(ConvertPersonToPersonResponse).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
            {
                return null; 
            }

            Person? person = await _db.People.Include("Country")
                .FirstOrDefaultAsync(temp => temp.PersonID == personID);
            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetFilteredPeople(string searchBy, string? searchString)
        {
            List<PersonResponse> allPeople = await GetAllPeople();
            List<PersonResponse> matchingPeople = allPeople;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPeople;
            }

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPeople = allPeople.Where(temp => 
                        (string.IsNullOrEmpty(temp.PersonName) || temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); ;
                    break;

                case nameof(PersonResponse.Email):
                    matchingPeople = allPeople.Where(temp =>
                        (string.IsNullOrEmpty(temp.Email) || temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); ;
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPeople = allPeople.Where(temp =>
                        (temp.DateOfBirth == null || temp.DateOfBirth.Value.ToString("dd MMMM yyy").Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); ;
                    break;

                case nameof(PersonResponse.Gender):
                    matchingPeople = allPeople.Where(temp =>
                        (string.IsNullOrEmpty(temp.Gender) || temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); ;
                    break;

                case nameof(PersonResponse.CountryID):
                    matchingPeople = allPeople.Where(temp =>
                        (string.IsNullOrEmpty(temp.Country) || temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); ;
                    break;

                case nameof(PersonResponse.Address):
                    matchingPeople = allPeople.Where(temp =>
                        (string.IsNullOrEmpty(temp.Address) || temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); ;
                    break;
                default:
                    matchingPeople = allPeople;
                    break;
            }

            return matchingPeople;
        }

        public async Task<List<PersonResponse>> GetSortedPeople(List<PersonResponse> allPeople, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return allPeople;
            }

            List<PersonResponse> sortedPeople = (sortBy, sortOrder)
                switch
                {
                    (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) 
                        => allPeople.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) 
                        => allPeople.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Email), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.Email).ToList(),

                    (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                        => allPeople.OrderByDescending(temp => temp.Email).ToList(),

                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.DateOfBirth).ToList(),

                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                        => allPeople.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                    (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.Age).ToList(),

                    (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                        => allPeople.OrderByDescending(temp => temp.Age).ToList(),

                    (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
                        => allPeople.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Country), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Country), SortOrderOptions.DESC)
                        => allPeople.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                        => allPeople.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC)
                        => allPeople.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC)
                        => allPeople.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                    _ => allPeople
                };

            return sortedPeople;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //validation 
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = await _db.People.FirstOrDefaultAsync(temp => temp.PersonID == personUpdateRequest.PersonID);

            if (matchingPerson == null)
            {
                throw new ArgumentException("Given person doesn't exist");
            }

            //Update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            await _db.SaveChangesAsync(); //UPDATE
            return matchingPerson.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }

            Person? person = await _db.People.FirstOrDefaultAsync(temp => temp.PersonID == personID);
            if (person == null)
            {
                return false; 
            }

            _db.People.Remove(await _db.People.FirstAsync(temp => temp.PersonID == personID));
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<MemoryStream> GetPeopleCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

            csvWriter.WriteHeader<PersonResponse>(); // PersonID, PersonName, ...
            csvWriter.NextRecord();
            csvWriter.WriteRecordsAsync()
        }
    }
}
