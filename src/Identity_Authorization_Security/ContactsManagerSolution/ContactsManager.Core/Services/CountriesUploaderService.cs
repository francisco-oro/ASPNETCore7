using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace ContactsManager.Core.Services
{
    public class CountriesUploaderService : ICountriesUploaderService
    {
        // private field 
        private readonly ICountriesRepository _countriesRepository;

        //constructor
        public CountriesUploaderService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream stream = new MemoryStream();
            await formFile.CopyToAsync(stream);

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Countries"];

                int rowCount = worksheet.Dimension.Rows;
                int countriesInserted = 0;
                for (int i = 2; i <= rowCount; i++)
                {
                    string? cellValue = Convert.ToString(worksheet.Cells[i, 1].Value);

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string? countryName = cellValue;

                        var countryByCountryName = _countriesRepository.GetCountryByCountryName(countryName);
                        if (countryByCountryName == null)
                        {
                            Country country = new Country() { CountryName = countryName};

                            await _countriesRepository.AddCountry(country);

                            countriesInserted++;
                        }
                    }
                }

                return countriesInserted;
            }
        }
    }
}
