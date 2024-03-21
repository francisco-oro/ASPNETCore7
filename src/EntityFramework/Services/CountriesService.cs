using System.Collections;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        // private field 
        private readonly PeopleDbContext _db;

        //constructor
        public CountriesService(PeopleDbContext peopleDbContext)
        {
            _db = peopleDbContext;
        }
        public async Task<CountryResponse?> AddCountry(CountryAddRequest? countryAddRequest)
        {
            // Validation: countryAddRequest parameter can't be null 
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Validation: countryName can't be null
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            // Validation: CountryName can't be duplicated
            if (await _db.Countries.CountAsync(temp => temp.CountryName != null && temp.CountryName.Equals(countryAddRequest.CountryName)) > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type 
            Country country = countryAddRequest.ToCountry();

            //generate CountryID
            country.CountryID = Guid.NewGuid();

            //Add country object into _db
            _db.Add(country);
            await _db.SaveChangesAsync();
            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await _db.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryId)
        {
            if (countryId == null)
            {
                return null;
            }

            Country? countryResponseFromList = await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryID == countryId);

            if (countryResponseFromList == null)
            {
                return null;
            }

            return countryResponseFromList.ToCountryResponse();
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

                        if (_db.Countries.Count(temp => temp.CountryName == countryName) == 0)
                        {
                            Country country = new Country() { CountryName = countryName};

                            _db.Countries.Add(country);
                            await _db.SaveChangesAsync();

                            countriesInserted++;
                        }
                    }
                }

                return countriesInserted;
            }
        }
    }
}
