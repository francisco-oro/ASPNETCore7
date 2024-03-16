
using System.ComponentModel.DataAnnotations;
using Entities;
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

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
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
            _db.SaveChanges();
            // Convert the Person object into PersonResponse type
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPeople()
        {
            return _db.People.Select(temp => ConvertPersonToPersonResponse(temp)).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
            {
                return null; 
            }

            Person? person = _db.People.FirstOrDefault(temp => temp.PersonID == personID);
            if (person == null)
            {
                return null;
            }

            PersonResponse personResponse = ConvertPersonToPersonResponse(person);
            return personResponse;
        }

        public List<PersonResponse> GetFilteredPeople(string searchBy, string? searchString)
        {
            List<PersonResponse> allPeople = GetAllPeople();
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

        public List<PersonResponse> GetSortedPeople(List<PersonResponse> allPeople, string sortBy, SortOrderOptions sortOrder)
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

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //validation 
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = _db.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);

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

            return matchingPerson.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }

            Person? person = _db.FirstOrDefault(temp => temp.PersonID == personID);
            if (person == null)
            {
                return false; 
            }

            _db.RemoveAll(temp => temp.PersonID == personID);

            return true;
        }
    }
}
