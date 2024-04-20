﻿using System.Drawing;
using System.Globalization;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;
using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Exceptions;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Serilog;
using SerilogTimings;


namespace Services
{
    public class PeopleGetterService : IPeopleGetterService
    {
        //private fields
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<PeopleGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        //constructor 

        public PeopleGetterService(IPeopleRepository peopleRepository, ILogger<PeopleGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        public virtual async Task<List<PersonResponse>> GetAllPeople()
        {
            _logger.LogInformation("GetAllPeople of PeopleGetterService");
            // SELECT * from People
            var people = await _peopleRepository.GetAllPeople();

            return people
                .Select(temp => temp.ToPersonResponse()).ToList();

        }

        public virtual async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
            {
                return null;
            }

            Person? person = await _peopleRepository.GetPersonByPersonID(personID.Value);

            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        public virtual async Task<List<PersonResponse>> GetFilteredPeople(string searchBy, string? searchString)
        {
            List<Person> people;
            _logger.LogInformation("GetFilteredPeople of PeopleGetterService");
            using (Operation.Time("Time for filtered people from Database"))
            {

                people = searchBy switch
                {
                    nameof(PersonResponse.PersonName) =>
                        await _peopleRepository.GetFilteredPeople(temp =>
                            temp.PersonName.Contains(searchString)),

                    nameof(PersonResponse.Email) =>
                        await _peopleRepository.GetFilteredPeople(temp =>
                            temp.Email.Contains(searchString)),

                    nameof(PersonResponse.DateOfBirth) =>
                        await _peopleRepository.GetFilteredPeople(temp =>
                            temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(
                                searchString)),

                    nameof(PersonResponse.Gender) =>
                        await _peopleRepository.GetFilteredPeople(temp =>
                            temp.Gender.Contains(searchString)),

                    nameof(PersonResponse.CountryID) =>
                        await _peopleRepository.GetFilteredPeople(temp =>
                            temp.Country.CountryName.Contains(searchString)),

                    nameof(PersonResponse.Address) =>
                        await _peopleRepository.GetFilteredPeople(temp =>
                            temp.Address.Contains(searchString)),

                    _ => await _peopleRepository.GetAllPeople()
                };
            }

            _diagnosticContext.Set("People", people);

            return people.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public virtual async Task<MemoryStream> GetPeopleCSV()
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

            List<PersonResponse> people = await GetAllPeople();

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

        public virtual async Task<MemoryStream> GetPeopleExcel()
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
                List<PersonResponse> people = await GetAllPeople();
                if (people.Count == 0)
                {
                    throw new InvalidOperationException("No people data");
                }
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
