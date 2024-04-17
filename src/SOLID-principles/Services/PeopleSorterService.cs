using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Serilog;


namespace Services
{
    public class PeopleSorterService : IPeopleSorterService
    {
        //private fields
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<PeopleSorterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        //constructor 

        public PeopleSorterService(IPeopleRepository peopleRepository, ILogger<PeopleSorterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        public async Task<List<PersonResponse>> GetSortedPeople(List<PersonResponse> allPeople, string sortBy, SortOrderOptions sortOrder)
        {
            _logger.LogInformation("GetSortedPeople of PeopleGetterService");
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
    }
}
