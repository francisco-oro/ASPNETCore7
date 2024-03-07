
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        //private fields
        private readonly List<Person> _people;
        private readonly ICountriesService _countriesService;
        //constructor 
        public PersonService()
        {
            _people = new List<Person>();
            _countriesService = new CountriesService();
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
            _people.Add(person);

            // Convert the Person object into PersonResponse type
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPeople()
        {
            return _people.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
            {
                return null; 
            }

            Person? person = _people.FirstOrDefault(temp => temp.PersonID == personID);
            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
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
                case nameof(Person.PersonName):
                    matchingPeople = allPeople.Where(temp => 
                        (string.IsNullOrEmpty(temp.PersonName) || temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); ;
                    break;

                case nameof(Person.Email):
                    matchingPeople = allPeople.Where(temp =>
                        (string.IsNullOrEmpty(temp.Email) || temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); ;
                    break;

                case nameof(Person.DateOfBirth):
                    matchingPeople = allPeople.Where(temp =>
                        (temp.DateOfBirth == null || temp.DateOfBirth.Value.ToString("dd MMMM yyy").Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); ;
                    break;

                case nameof(Person.Gender):
                    matchingPeople = allPeople.Where(temp =>
                        (string.IsNullOrEmpty(temp.Gender) || temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList(); ;
                    break;
            }
        }
    }
}
