
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
        private readonly List<Person> _people;
        private readonly ICountriesService _countriesService;
        //constructor 
        public PersonService(bool initialize = true)
        {
            _people = new List<Person>();
            _countriesService = new CountriesService();

            if (initialize)
            {
                _people.AddRange(new List<Person>()
                {
                    new Person() { PersonID = Guid.Parse("AF2F5711-6D77-4DA3-9888-F1F50E95A868"), PersonName = "Rita", Email="rfilyakov0@indiegogo.com", DateOfBirth = DateTime.Parse("1994-02-16"), Gender = "Female", Address = "560 Quincy Terrace", ReceiveNewsLetters = true, CountryID = Guid.Parse("18A3216C-0915-443C-9530-B688390502F7")},
                    new Person() { PersonID = Guid.Parse("1579EC0E-D3D0-4C3E-9AA8-5987F328C3DE"), PersonName = "Sindee", Email="smeharg1@illinois.edu", DateOfBirth = DateTime.Parse("1991-02-08"), Gender = "Female", Address = "4 Jenifer Way", ReceiveNewsLetters = false, CountryID = Guid.Parse("41E5F973-A4A5-4540-9FFD-9716FE8D5CE5")},
                    new Person() { PersonID = Guid.Parse("0A0EE9B5-AB0F-4728-8F83-D9F4D0E59DA8"), PersonName = "Konstance", Email="kkilliner2@google.co",DateOfBirth = DateTime.Parse("1990-12-26"), Gender = "Female", Address = "81345 Del Mar Alley", ReceiveNewsLetters = true, CountryID = Guid.Parse("19E5BA7E-F942-4CD9-85F7-6EB16182E165")}, 
                    new Person() { PersonID = Guid.Parse("DD7D8297-4FBB-49B0-A453-5FC14AD53F75"), PersonName = "Virgilio", Email="vmcteer3@omniture.com", DateOfBirth = DateTime.Parse("2000-09-22"), Gender = "Male", Address = "8 Towne Road", ReceiveNewsLetters = false, CountryID = Guid.Parse("E83B824C-C68D-4AF3-8143-CF2F214F7FCE")},
                    new Person() { PersonID = Guid.Parse("735D6F0E-A12C-457E-937D-A571922B5655"), PersonName = "Caty", Email="cgilhouley4@multiply.com", DateOfBirth = DateTime.Parse("2000-04-11"), Gender = "Female", Address = "326 Eastwood Way", ReceiveNewsLetters = false, CountryID = Guid.Parse("3B3E2AFF-9767-4DB5-A57C-EFAB58F6E9CE")},
                    new Person() { PersonID = Guid.Parse("3988A11B-4942-413A-B08D-C6A8B9BF4E53"), PersonName = "Noreen", Email="nmatijevic5@amazon.de", DateOfBirth = DateTime.Parse("1998-04-03"), Gender = "Female", Address = "4 Miller Plaza", ReceiveNewsLetters = true, CountryID = Guid.Parse("18A3216C-0915-443C-9530-B688390502F7")},
                    new Person() { PersonID = Guid.Parse("C44A255C-31CD-46E9-B219-2332900DF099"), PersonName = "Vale", Email="vsemeniuk6@cpanel.net", DateOfBirth = DateTime.Parse("1996-03-08"), Gender = "Female", Address = "679 Del Mar Parkway", ReceiveNewsLetters = true, CountryID = Guid.Parse("41E5F973-A4A5-4540-9FFD-9716FE8D5CE5")},
                    
                });
            }
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
            return _people.Select(temp =>
            {
                PersonResponse person = temp.ToPersonResponse();

                // People's country search functionality 
                person.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
                return person;
            }).ToList();
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
            Person? matchingPerson = _people.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);

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

            Person? person = _people.FirstOrDefault(temp => temp.PersonID == personID);
            if (person == null)
            {
                return false; 
            }

            _people.RemoveAll(temp => temp.PersonID == personID);

            return true;
        }
    }
}
