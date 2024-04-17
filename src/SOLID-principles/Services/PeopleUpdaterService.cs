using Entities;
using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using Serilog;


namespace Services
{
    public class PeopleUpdaterService : IPeopleUpdaterService
    {
        //private fields
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<PeopleUpdaterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        //constructor 

        public PeopleUpdaterService(IPeopleRepository peopleRepository, ILogger<PeopleUpdaterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
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
            Person? matchingPerson = await _peopleRepository.GetPersonByPersonID(personUpdateRequest.PersonID);

            if (matchingPerson == null)
            {
                throw new InvalidPersonIDException("Given person doesn't exist");
            }

            //Update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            await _peopleRepository.UpdatePerson(matchingPerson); //UPDATE
            return matchingPerson.ToPersonResponse();
        }
    }
}
