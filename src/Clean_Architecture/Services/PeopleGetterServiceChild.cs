using OfficeOpenXml;
using ServiceContracts.DTO;
using System.Drawing;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;

namespace Services
{
    public class PeopleGetterServiceChild : PeopleGetterService
    {
        public PeopleGetterServiceChild(IPeopleRepository peopleRepository, ILogger<PeopleGetterService> logger, IDiagnosticContext diagnosticContext) : base(peopleRepository, logger, diagnosticContext)
        {
        }

        public override async Task<MemoryStream> GetPeopleExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("PeopleSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Age";
                worksheet.Cells["C1"].Value = "Gender";

                using (ExcelRange range = worksheet.Cells["A1:C1"])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    range.Style.Font.Bold = true;
                }
                int row = 2;
                List<PersonResponse> people = await GetAllPeople();

                foreach (PersonResponse personResponse in people)
                {
                    worksheet.Cells[row, 1].Value = personResponse.PersonName;
                    worksheet.Cells[row, 2].Value = personResponse.Age;
                    worksheet.Cells[row, 3].Value = personResponse.Gender;
                    row++;
                }

                worksheet.Cells[$"A1:C{row}"].AutoFitColumns();

                await package.SaveAsync();

                memoryStream.Position = 0;
                return memoryStream;
            }
        }
    }
}
