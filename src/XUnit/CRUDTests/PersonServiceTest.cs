using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit; 
namespace CRUDTests
{
    public class PersonServiceTest
    {
        // private fields
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        // constructor 
        public PersonServiceTest()
        {
            _personService = new PersonService();
            _countriesService = new CountriesService();
        }

        #region AddPerson

        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Act 
            Assert.Throws<ArgumentNullException>(() =>
            { 
                _personService.AddPerson(personAddRequest);
            });

        }

        //When we supply null value as null value as PersonName, it should throw ArgumentException
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange 
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = null
            };

            // Act 
            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });
        }

        //When we supply proper person details, it should insert the person into the people list; 
        // it should return an object of PersonResponse, which includes with the newly generated person id
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange 
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person name...",
                Email = "person@example.com", 
                Address = "Sample address", 
                CountryID = Guid.NewGuid(), 
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            // Act 
            PersonResponse person_response_from_add = _personService.AddPerson(personAddRequest);
            List<PersonResponse> people_list = _personService.GetAllPeople();
            //Assert
            Assert.True(person_response_from_add.PersonID != Guid.Empty);
            Assert.Contains(person_response_from_add, people_list); 
        }


        #endregion

        #region GetPersonByPersonID

        // If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            // Arrange 
            Guid? personID = null; 

            //Act
            PersonResponse? personResponseFromGet = _personService.GetPersonByPersonID(personID);

            //Assert 
            Assert.Null(personResponseFromGet);
        }

        // If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public void GetPersonByPersonID_WithPersonID()
        {
            //Arrange 
            CountryAddRequest country_request = new CountryAddRequest()
            {
                CountryName = "Canada"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(country_request);

            // Act 
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person name...",
                Email = "email@sample.com",
                Address = "address",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };

            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);
            PersonResponse personResponseFromGet = _personService.GetPersonByPersonID(sd)
            //Assert 

        }
        #endregion
    }
}
