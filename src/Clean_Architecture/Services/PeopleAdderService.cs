using System.Drawing;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Exceptions;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using Serilog;
using SerilogTimings;


namespace Services
{
    public class PeopleAdderService : IPeopleAdderService
    {
        //private fields
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<PeopleAdderService> _logger;

        private readonly IDiagnosticContext _diagnosticContext;
        //constructor 

        public PeopleAdderService(IPeopleRepository peopleRepository, ILogger<PeopleAdderService> logger,
            IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
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
            await _peopleRepository.AddPerson(person);
            // Convert the Person object into PersonResponse type
            return person.ToPersonResponse();
        }
    }
}
