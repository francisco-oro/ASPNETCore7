using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonServiceTest
    {
        // private fields
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _outputHelper;

        // constructor 
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _countriesService = new CountriesService(
                new PeopleDbContext(new DbContextOptionsBuilder<PeopleDbContext>().Options));

            _personService = new PersonService(new PeopleDbContext(
                new DbContextOptionsBuilder<PeopleDbContext>().Options), _countriesService);
            _outputHelper = testOutputHelper;
        }

        // helper methods
        private async Task<(CountryResponse? countryResponse1, CountryResponse? countryResponse2)> AddCountries()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "India"
            };

            CountryResponse? countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse? countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);
            return (countryResponse1, countryResponse2);
        }

        private async Task<List<PersonResponse>> AddPeople(CountryResponse countryResponse1, CountryResponse countryResponse2)
        {
            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Gender = GenderOptions.Male,
                Address = "address of smith",
                CountryID = countryResponse1.CountryID,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "Mary@example.com",
                Gender = GenderOptions.Female,
                Address = "address of Mary",
                CountryID = countryResponse2.CountryID,
                DateOfBirth = DateTime.Parse("2000-02-02"),
                ReceiveNewsLetters = false
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                Email = "Rahman@example.com",
                Gender = GenderOptions.Male,
                Address = "address of Rahman",
                CountryID = countryResponse1.CountryID,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> personRequests = new List<PersonAddRequest>()
            {
                personAddRequest1, personAddRequest2, personAddRequest3
            };

            List<PersonResponse> personResponsesFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest personRequest in personRequests)
            {
                PersonResponse personResponse = await _personService.AddPerson(personRequest);
                personResponsesFromAdd.Add(personResponse);
            }

            return personResponsesFromAdd;
        }

        #region AddPerson

        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Act 
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            { 
                await _personService.AddPerson(personAddRequest);
            });

        }

        //When we supply null value as null value as PersonName, it should throw ArgumentException
        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            //Arrange 
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = null
            };

            // Act 
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _personService.AddPerson(personAddRequest);
            });
        }

        //When we supply proper person details, it should insert the person into the people list; 
        // it should return an object of PersonResponse, which includes with the newly generated person id
        [Fact]
        public async Task AddPerson_ProperPersonDetails()
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
            PersonResponse personResponseFromAdd = await _personService.AddPerson(personAddRequest);
            List<PersonResponse> peopleList = await _personService.GetAllPeople();
            //Assert
            Assert.True(personResponseFromAdd.PersonID != Guid.Empty);
            Assert.Contains(personResponseFromAdd, peopleList); 
        }


        #endregion

        #region GetPersonByPersonID

        // If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID()
        {
            // Arrange 
            Guid? personID = null; 

            //Act
            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonID(personID);

            //Assert 
            Assert.Null(personResponseFromGet);
        }

        // If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public async Task GetPersonByPersonID_WithPersonID()
        {
            //Arrange 
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Canada"
            };
            CountryResponse? countryResponse = await _countriesService.AddCountry(countryRequest);

            // Act 
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person name...",
                Email = "email@sample.com",
                Address = "address",
                CountryID = countryResponse?.CountryID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };

            PersonResponse personResponseFromAdd = await _personService.AddPerson(personAddRequest);
            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonID(personResponseFromAdd.PersonID);
            //Assert 
            Assert.Equal(personResponseFromGet, personResponseFromAdd);
        }
        #endregion

        #region GetAllPeople

        // The GetAllPeople should return an empty list by default
        [Fact]
        public async Task GetAllPeople_EmptyList()
        {
            //Act 
            List<PersonResponse> peopleFromGet = await _personService.GetAllPeople();

            //Assert 
            Assert.Empty(peopleFromGet);
        }

        // First, we will add few people; and then when we call GetAllPeople(), it should
        // return the same people that were added 
        [Fact]
        public async Task GetAllPeople_AddFewPeople()
        {
            //Arrange
            var (countryResponse1, countryResponse2) = await AddCountries();
            var personResponsesFromAdd = await AddPeople(countryResponse1, countryResponse2);

            //print personResponsesFromAdd 
            _outputHelper.WriteLine("Expected:");
            foreach (var personResponse in personResponsesFromAdd)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            //Act
            List<PersonResponse> peopleListFromGet = await _personService.GetAllPeople();


            //print peopleListFromGet 
            _outputHelper.WriteLine("Actual:");
            foreach (var personResponse in peopleListFromGet)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            //Assert 
            foreach (var personResponse in personResponsesFromAdd)
            {
                Assert.Contains(personResponse, peopleListFromGet);
            }
        }
        #endregion

        #region GetFilteredPeople
        //If the search text is empty and search by is "PersonName", it should return all people
        [Fact]
        public async Task GetAllPeople_EmptySearchText()
        {
            //Arrange
            var (countryResponse1, countryResponse2) = await AddCountries();
            var personResponsesFromAdd = await AddPeople(countryResponse1, countryResponse2);

            //print personResponsesFromAdd 
            _outputHelper.WriteLine("Expected:");
            foreach (var personResponse in personResponsesFromAdd)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            //Act
            List<PersonResponse> peopleListFromSearch = await _personService.GetFilteredPeople(nameof(Person.PersonName), "");

            //print peopleListFromGet 
            _outputHelper.WriteLine("Actual:");
            foreach (var personResponse in peopleListFromSearch)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            //Assert 
            foreach (var personResponse in personResponsesFromAdd)
            {
                Assert.Contains(personResponse, peopleListFromSearch);
            }
        }

        [Theory]
        [InlineData("ma")]
        [InlineData("sm")]
        [InlineData("m")]
        // First we will add few people; and then we will search based on person name with some search string. It should return the matching people
        public async Task GetAllPeople_SearchByPersonName(string searchString)
        {
            var (countryResponse1, countryResponse2) = await AddCountries();

            var personResponsesFromAdd = await AddPeople(countryResponse1, countryResponse2);

            //print personResponsesFromAdd 
            _outputHelper.WriteLine("Expected:");
            foreach (var personResponse in personResponsesFromAdd)
            {
                if (personResponse.PersonName == null)
                {
                    continue;
                }
                if (personResponse.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                {
                    _outputHelper.WriteLine(personResponse.ToString());
                }
            }

            //Act
            List<PersonResponse> peopleListFromSearch = await _personService.GetFilteredPeople(nameof(Person.PersonName), searchString);

            //print peopleListFromGet 
            _outputHelper.WriteLine("Actual:");
            foreach (var personResponse in peopleListFromSearch)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            //Assert 
            foreach (var personResponse in personResponsesFromAdd)
            {
                if (personResponse.PersonName == null)
                {
                    continue;
                }
                if (personResponse.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(personResponse, peopleListFromSearch);
                }
            }
        }



        #endregion

        #region GetSortedPeople
        // When we sort based on PersonName in DESC, it should return people list in descending on PersonName
        [Fact]
        public async Task GetSortedPeople()
        {
            //Arrange
            var (countryResponse1, countryResponse2) = await AddCountries();
            var personResponsesFromAdd = await AddPeople(countryResponse1, countryResponse2);

            //print personResponsesFromAdd 
            _outputHelper.WriteLine("Expected:");
            foreach (var personResponse in personResponsesFromAdd)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> allPeople = await _personService.GetAllPeople();
            //Act
            List<PersonResponse> peopleListFromSort = await _personService.GetSortedPeople(allPeople, nameof(Person.PersonName), SortOrderOptions.DESC);

            //print peopleListFromGet 
            _outputHelper.WriteLine("Actual:");
            foreach (var personResponse in peopleListFromSort)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }
            personResponsesFromAdd = personResponsesFromAdd.OrderByDescending(temp => temp.PersonName).ToList();
            //Assert 
            for (int i = 0; i < personResponsesFromAdd.Count; i++)
            {
                Assert.Equal(personResponsesFromAdd[i], peopleListFromSort[i]);
            }
        }

        #endregion

        #region UpdatePerson

        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act 
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }

        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //Arrange 
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                PersonID = Guid.NewGuid()
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act 
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }


        //When personName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            //Arrange 
            var (countryResponse1, countryResponse2) = await AddCountries();
            var personFromAdd = await AddPeople(countryResponse1, countryResponse2);

            PersonUpdateRequest personUpdateRequest = personFromAdd[0].ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            //Act
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _personService.UpdatePerson(personUpdateRequest);
            });

        }

        //First, add a new person and try to update the person name and email       
        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdate()
        {
            //Arrange 
            var (countryResponse1, countryResponse2) = await AddCountries();
            var personFromAdd = await AddPeople(countryResponse1, countryResponse2);

            PersonUpdateRequest personUpdateRequest = personFromAdd[0].ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "William";
            personUpdateRequest.Email = "william@example.com";

            //Act
            PersonResponse personResponseFromUpdatePerson = await _personService.UpdatePerson(personUpdateRequest);

            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonID(personResponseFromUpdatePerson.PersonID);

            //Assert 
            Assert.Equal(personResponseFromGet, personResponseFromUpdatePerson);
        }
        #endregion

        #region DeletePerson

        //If you supply an valid PersonID, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            //Arrange
            var (countryResponse1, countryResponse2) = await AddCountries();
            var peopleFromAdd = await AddPeople(countryResponse1, countryResponse2);

            //Act 
            bool isDeleted = await _personService.DeletePerson(peopleFromAdd[2].PersonID);

            //Assert
            Assert.True(isDeleted);
        }

        //If you supply an invalid PersonID, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            //Arrange
            var (countryResponse1, countryResponse2) = await AddCountries();
            var peopleFromAdd = await AddPeople(countryResponse1, countryResponse2);

            //Act 
            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

            //Assert
            Assert.False(isDeleted);
        }
        #endregion
    }
}
