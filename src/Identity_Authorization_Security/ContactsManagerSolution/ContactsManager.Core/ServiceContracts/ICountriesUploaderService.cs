using Microsoft.AspNetCore.Http;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesUploaderService
    {
        /// <summary>
        /// Uploads countries from excel file into database
        /// </summary>
        /// <param name="formFile">Returns number of countries added</param>
        /// <returns></returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
