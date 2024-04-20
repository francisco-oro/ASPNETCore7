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
    public class PeopleDeleterService : IPeopleDeleterService
    {
        //private fields
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<PeopleDeleterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        //constructor 

        public PeopleDeleterService(IPeopleRepository peopleRepository, ILogger<PeopleDeleterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }

            Person? person = await _peopleRepository.GetPersonByPersonID(personID.Value);
            if (person == null)
            {
                return false;
            }

            await _peopleRepository.DeletePersonByPersonID(personID.Value);
            return true;
        }
    }
}
