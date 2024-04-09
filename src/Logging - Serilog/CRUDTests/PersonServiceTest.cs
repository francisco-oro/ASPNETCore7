using System.Linq.Expressions;
using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RepositoryContracts;
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
        private readonly IPeopleService _peopleService;
        private readonly ICountriesService _countriesService;

        private readonly Mock<IPeopleRepository> _peopleRepositoryMock;
        private readonly IPeopleRepository _peopleRepository;
        
        private readonly ITestOutputHelper _outputHelper;
        private readonly IFixture _fixture;

        // constructor 
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _peopleRepositoryMock = new Mock<IPeopleRepository>();
            _peopleRepository = _peopleRepositoryMock.Object;

            var peopleInitialData = new List<Person>() { };
            if (peopleInitialData == null) throw new ArgumentNullException(nameof(peopleInitialData));
            var countriesInitialData = new List<Country>() { };
            if (countriesInitialData == null) throw new ArgumentNullException(nameof(countriesInitialData));

            DbContextMock<ApplicationDbContext> doContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            ApplicationDbContext dbContext = doContextMock.Object;
            doContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            doContextMock.CreateDbSetMock(temp => temp.People, peopleInitialData);

            _countriesService = new CountriesService(null);
            _peopleService = new PeopleService(_peopleRepository);
            _outputHelper = testOutputHelper;
        }

        // helper methods
        private async Task<(CountryResponse? countryResponse1, CountryResponse? countryResponse2)> AddCountries()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest countryAddRequest2 = _fixture.Create<CountryAddRequest>();

            CountryResponse? countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse? countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);
            return (countryResponse1, countryResponse2);
        }

        private async Task<List<PersonResponse>> AddPeople(CountryResponse countryResponse1, CountryResponse countryResponse2)
        {
            PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "guest1@exmaple.com")
                .With(p => p.PersonName, "Smith")
                .Create();
            PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "guest2@exmaple.com")
                .With(p => p.PersonName, "Maria")
                .Create();

            PersonAddRequest personAddRequest3 = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "guest3@exmaple.com")
                .Create();

            List<PersonAddRequest> personRequests = new List<PersonAddRequest>()
            {
                personAddRequest1, personAddRequest2, personAddRequest3
            };

            List<PersonResponse> personResponsesFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest personRequest in personRequests)
            {
                PersonResponse personResponse = await _peopleService.AddPerson(personRequest);
                personResponsesFromAdd.Add(personResponse);
            }

            return personResponsesFromAdd;
        }

        #region AddPerson

        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null; 
            //Act 
            Func<Task> action = async () =>
            { 
                await _peopleService.AddPerson(personAddRequest);
            };
            await action.Should().ThrowAsync<ArgumentNullException>(); 
        }

        //When we supply null value as null value as PersonName, it should throw ArgumentException
        [Fact]
        public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
        {
            //Arrange 
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();
            
            Person person = personAddRequest.ToPerson();

            // When PeopleRepository.AddPerson is called, it has tu return the same "person" object

            _peopleRepositoryMock
                .Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);
            // Act
            Func<Task> action = async() =>
            {
                await _peopleService.AddPerson(personAddRequest);
            }; 
            
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When we supply proper person details, it should insert the person into the people list; 
        // it should return an object of PersonResponse, which includes with the newly generated person id
        [Fact]
        public async Task AddPerson_ProperPersonDetails_ToBeSuccessful()
        {
            //Arrange 
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone@example.com")
                .Create();
            Person person = personAddRequest.ToPerson();
            PersonResponse personResponseExpected = person.ToPersonResponse();

            // If we supply anr argument value to the AddPerson method, it should return the same return value
            _peopleRepositoryMock.Setup(
                    temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);
            // Act 
            PersonResponse personResponseFromAdd = await _peopleService.AddPerson(personAddRequest);

            personResponseExpected.PersonID = personResponseFromAdd.PersonID;
            //Assert

            personResponseFromAdd.PersonID.Should().NotBe(Guid.Empty);
            personResponseFromAdd.Should().Be(personResponseExpected);
        }


        #endregion

        #region GetPersonByPersonID

        // If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID_ToBeNull()
        {
            // Arrange 
            Guid? personID = null; 

            //Act
            PersonResponse? personResponseFromGet = await _peopleService.GetPersonByPersonID(personID);

            //Assert 
            personResponseFromGet.Should().BeNull();
        }

        // If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public async Task GetPersonByPersonID_WithPersonID_ToBeSuccessful()
        {
            //Arrange 
            Person personRequest = _fixture.Build<Person>()
                .With(temp => temp.Email, "person@example.com")
                .With(temp => temp.Country, null as Country)
                .Create();
            PersonResponse personResponseExpected = personRequest.ToPersonResponse();

            _peopleRepositoryMock.Setup(temp =>
                    temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(personRequest);
            // Act 
            PersonResponse? personResponseFromGet = await _peopleService.GetPersonByPersonID(personRequest.PersonID); 



            //Assert 
            //Assert.Equal(personResponseFromGet, personResponseFromAdd);
            personResponseFromGet.Should().Be(personResponseExpected);
        }
        #endregion

        #region GetAllPeople

        // The GetAllPeople should return an empty list by default
        [Fact]
        public async Task GetAllPeople_EmptyList()
        {
            var people = new List<Person>();
            //Arrange 
            _peopleRepositoryMock.Setup(temp => temp.GetAllPeople())
                .ReturnsAsync(people);

            //Act 
            List<PersonResponse> peopleFromGet = await _peopleService.GetAllPeople();

            //Assert 
            //Assert.Empty(peopleFromGet);
            peopleFromGet.Should().BeEmpty();
        }

        // First, we will add few people; and then when we call GetAllPeople(), it should
        // return the same people that were added 
        [Fact]
        public async Task GetAllPeople_WithFewPeople_ToBeSuccessful()
        {
            //Arrange
            List<Person> people = new List<Person>();
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.Country, null as Country)
                .Create();
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_2@example.com")
                .With(temp => temp.Country, null as Country)
                .Create();
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_3@example.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            List<PersonResponse> personResponseListExpected = 
                people.Select(temp => temp.ToPersonResponse()).ToList();
            //print personResponsesFromAdd 
            _outputHelper.WriteLine("Expected:");
            foreach (var personResponse in personResponseListExpected)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            _peopleRepositoryMock.Setup(temp => temp.GetAllPeople())
                .ReturnsAsync(people);
            //Act
            List<PersonResponse> peopleListFromGet = await _peopleService.GetAllPeople();


            //print peopleListFromGet 
            _outputHelper.WriteLine("Actual:");
            foreach (var personResponse in peopleListFromGet)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            //Assert 

            peopleListFromGet.Should().BeEquivalentTo(personResponseListExpected);
        }
        #endregion

        #region GetFilteredPeople
        //If the search text is empty and search by is "PersonName", it should return all people
        [Fact]
        public async Task GetFilteredPeople_EmptySearchText_ToBeSuccessful()
        {
            //Arrange
            List<Person> people = new List<Person>();
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.Country, null as Country)
                .Create();
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_2@example.com")
                .With(temp => temp.Country, null as Country)
                .Create();
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_3@example.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            List<PersonResponse> personResponseListExpected =
                people.Select(temp => temp.ToPersonResponse()).ToList();

            _peopleRepositoryMock.Setup(temp =>
                    temp.GetFilteredPeople(It.IsAny<Expression<Func<Person, bool>>>())).
                ReturnsAsync(people);
            //print personResponsesFromAdd 
            _outputHelper.WriteLine("Expected:");
            foreach (var personResponse in personResponseListExpected)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            //Act
            List<PersonResponse> peopleListFromSearch = await _peopleService.GetFilteredPeople(nameof(Person.PersonName), "");

            //print peopleListFromGet 
            _outputHelper.WriteLine("Actual:");
            foreach (var personResponse in peopleListFromSearch)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            //Assert 
            peopleListFromSearch.Should().BeEquivalentTo(personResponseListExpected);
        }

        [Theory]
        [InlineData("ma")]
        [InlineData("sm")]
        [InlineData("m")]
        // First we will add few people; and then we will search based on person name with some search string. It should return the matching people
        public async Task GetFilteredPeople_SearchByPersonName_ToBeSuccessFull(string searchString)
        {
            //Arrange
            List<Person> people = new List<Person>();
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.Country, null as Country)
                .Create();
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_2@example.com")
                .With(temp => temp.PersonName, "Maria")
                .With(temp => temp.Country, null as Country)
                .Create();
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_3@example.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            List<PersonResponse> personResponseListExpected =
                people.Select(temp => temp.ToPersonResponse()).ToList();

            _peopleRepositoryMock.Setup(temp =>
                    temp.GetFilteredPeople(It.IsAny<Expression<Func<Person, bool>>>())).
                ReturnsAsync(people);
            //print personResponsesFromAdd 
            _outputHelper.WriteLine("Expected:");
            foreach (var personResponse in personResponseListExpected)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            //Act
            List<PersonResponse> peopleListFromSearch = await _peopleService.GetFilteredPeople(nameof(Person.PersonName), searchString);

            //print peopleListFromGet 
            _outputHelper.WriteLine("Actual:");
            foreach (var personResponse in peopleListFromSearch)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            //Assert 
            peopleListFromSearch.Should().BeEquivalentTo(personResponseListExpected);
        }



        #endregion

        #region GetSortedPeople
        // When we sort based on PersonName in DESC, it should return people list in descending on PersonName
        [Fact]
        public async Task GetSortedPeople_ToBeSuccessful()
        {
            //Arrange
            List<Person> people = new List<Person>();
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.Country, null as Country)
                .Create();
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_2@example.com")
                .With(temp => temp.PersonName, "Maria")
                .With(temp => temp.Country, null as Country)
                .Create();
            _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_3@example.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            List<PersonResponse> personResponseListExpected =
                people.Select(temp => temp.ToPersonResponse()).ToList();

            _peopleRepositoryMock
                .Setup(temp => temp.GetAllPeople())
                .ReturnsAsync(people);
            //print personResponsesFromAdd 
            _outputHelper.WriteLine("Expected:");
            foreach (var personResponse in personResponseListExpected)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }

            List<PersonResponse> allPeople = await _peopleService.GetAllPeople();
            //Act
            List<PersonResponse> peopleListFromSort = await _peopleService.GetSortedPeople(allPeople, nameof(Person.PersonName), SortOrderOptions.DESC);

            //print peopleListFromGet 
            _outputHelper.WriteLine("Actual:");
            foreach (var personResponse in peopleListFromSort)
            {
                _outputHelper.WriteLine(personResponse.ToString());
            }
            //Assert 
            peopleListFromSort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }

        #endregion

        #region UpdatePerson

        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            //Assert
            Func<Task> action = async () =>
            {
                //Act 
                await _peopleService.UpdatePerson(personUpdateRequest);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonID_ToBeArgumentException()
        {
            //Arrange 
            PersonUpdateRequest? personUpdateRequest = _fixture.Build<PersonUpdateRequest>()
                .With(temp => temp.Email, "person@example.com")
                .Create();

            //Assert
            Func<Task> action = async () =>
            {
                //Act 
                await _peopleService.UpdatePerson(personUpdateRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }


        //When personName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
        {
            //Arrange
            Person person = _fixture.Build<Person>()
                .With(temp => temp.PersonName, null as string)
                .With(temp => temp.Email, "someone@example.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();
            PersonResponse personResponse = person.ToPersonResponse();
            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            //Act
            Func<Task> action = async () =>
            {
                //Act
                await _peopleService.UpdatePerson(personUpdateRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();

        }

        //First, add a new person and try to update the person name and email       
        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdate_ToBeSuccessful()
        {
            // Arrange 
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "someone@example.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();

            PersonResponse personResponse = person.ToPersonResponse();
            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            _peopleRepositoryMock
                .Setup(temp => temp.UpdatePerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            _peopleRepositoryMock
                .Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            //Act
            PersonResponse personResponseFromUpdatePerson = await _peopleService.UpdatePerson(personUpdateRequest);

            PersonResponse? personResponseFromGet = await _peopleService.GetPersonByPersonID(personResponseFromUpdatePerson.PersonID);

            //Assert 
            personResponseFromUpdatePerson.Should().Be(personResponseFromGet);
        }
        #endregion

        #region DeletePerson

        //If you supply an valid PersonID, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonID_ToBeSuccessful()
        {
            // Arrange
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "someone@example.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();

            _peopleRepositoryMock
                .Setup(temp => temp.DeletePersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            _peopleRepositoryMock
                .Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(person);
            //Act 
            bool isDeleted = await _peopleService.DeletePerson(person.PersonID);

            //Assert
            isDeleted.Should().BeTrue();
        }

        //If you supply an invalid PersonID, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            // Arrange
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "someone@example.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();

            _peopleRepositoryMock
                .Setup(temp => temp.DeletePersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            _peopleRepositoryMock
                .Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(null as Person);
            //Act 
            bool isDeleted = await _peopleService.DeletePerson(person.PersonID);

            //Assert
            isDeleted.Should().BeFalse();
        }
        #endregion
    }
}
