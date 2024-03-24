
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        //private fields
        private readonly IPeopleRepository _peopleRepository;
        private readonly ICountriesService _countriesService;
        
        //constructor 
        public PersonService(ApplicationDbContext peopleRepository, ICountriesService countriesService)
        {
            _peopleRepository = peopleRepository;
            _countriesService = countriesService;
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
            _peopleRepository.People.Add(person);
            await _peopleRepository.SaveChangesAsync();
            //_peopleRepository.sp_InsertPerson(person);
            // Convert the Person object into PersonResponse type
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetAllPeople()
        {
            // SELECT * from People
            var people =await  _peopleRepository.People.Include("Country").ToListAsync();

            return people
                .Select(temp => temp.ToPersonResponse()).ToList();

            //return _peopleRepository.sp_GetAllPeople()
            //    .Select(ConvertPersonToPersonResponse).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
            {
                return null; 
            }

            Person? person = await _peopleRepository.People.Include("Country")
                .FirstOrDefaultAsync(temp => temp.PersonID == personID);
            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetFilteredPeople(string searchBy, string? searchString)
        {
            List<PersonResponse> allPeople = await GetAllPeople();
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

        public async Task<List<PersonResponse>> GetSortedPeople(List<PersonResponse> allPeople, string sortBy, SortOrderOptions sortOrder)
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

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //validation 
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = await _peopleRepository.People.FirstOrDefaultAsync(temp => temp.PersonID == personUpdateRequest.PersonID);

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

            await _peopleRepository.SaveChangesAsync(); //UPDATE
            return matchingPerson.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }

            Person? person = await _peopleRepository.People.FirstOrDefaultAsync(temp => temp.PersonID == personID);
            if (person == null)
            {
                return false; 
            }

            _peopleRepository.People.Remove(await _peopleRepository.People.FirstAsync(temp => temp.PersonID == personID));
            await _peopleRepository.SaveChangesAsync();
            return true;
        }

        public async Task<MemoryStream> GetPeopleCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);

            // PersonName, Email
            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Age));
            csvWriter.WriteField(nameof(PersonResponse.Country));
            csvWriter.WriteField(nameof(PersonResponse.Gender));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
            await csvWriter.NextRecordAsync();

            List<PersonResponse> people = await _peopleRepository.People
                .Include("Country")
                .Select(temp => temp.ToPersonResponse()).ToListAsync();

            foreach (var personResponse in people)
            {
                csvWriter.WriteField(personResponse.PersonName);
                csvWriter.WriteField(personResponse.Email);
                csvWriter.WriteField(personResponse.DateOfBirth?.ToString("yyyy-MM-dd"));
                csvWriter.WriteField(personResponse.Age);
                csvWriter.WriteField(personResponse.Country);
                csvWriter.WriteField(personResponse.Gender);
                csvWriter.WriteField(personResponse.Address);
                csvWriter.WriteField(personResponse.ReceiveNewsLetters);
                await csvWriter.NextRecordAsync();

                await csvWriter.FlushAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<MemoryStream> GetPeopleExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("PeopleSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Date of birth";
                worksheet.Cells["D1"].Value = "Age";
                worksheet.Cells["E1"].Value = "Gender";
                worksheet.Cells["F1"].Value = "Country";
                worksheet.Cells["G1"].Value = "Address";
                worksheet.Cells["H1"].Value = "Receive News Letters";

                using (ExcelRange range = worksheet.Cells["A1:H1"])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    range.Style.Font.Bold = true;
                }
                int row = 2;
                List<PersonResponse> people = await _peopleRepository.People.Include("Country").Select(temp => temp.ToPersonResponse())
                    .ToListAsync();

                foreach (PersonResponse personResponse in people)
                {
                    worksheet.Cells[row, 1].Value = personResponse.PersonName;
                    worksheet.Cells[row, 2].Value = personResponse.Email;
                    if (personResponse.DateOfBirth != null)
                        worksheet.Cells[row, 3].Value = personResponse.DateOfBirth.Value.ToString("yyy-MM-dd");
                    worksheet.Cells[row, 4].Value = personResponse.Age;
                    worksheet.Cells[row, 5].Value = personResponse.Gender;
                    worksheet.Cells[row, 6].Value = personResponse.Country;
                    worksheet.Cells[row, 7].Value = personResponse.Address;
                    worksheet.Cells[row, 8].Value = personResponse.ReceiveNewsLetters;

                    row++;
                }

                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await package.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream; 
        }
    }
}
