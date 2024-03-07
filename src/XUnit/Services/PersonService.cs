
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
            throw new NotImplementedException();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            throw new NotImplementedException();
        }
    }
}
