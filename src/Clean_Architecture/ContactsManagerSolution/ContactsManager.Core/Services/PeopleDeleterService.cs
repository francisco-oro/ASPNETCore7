using System.Drawing;
using System.Globalization;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.ServiceContracts;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;


namespace ContactsManager.Core.Services
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
