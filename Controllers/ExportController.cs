using EduSat.TestSeries.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace EduSat.TestSeries.Service.Controllers
{
    [ApiController]
    [Route("api/export")]
    public class ExportController : ControllerBase
    {
        private readonly IReportsService _reportsService;


        public ExportController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpGet("excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            // Fetch the data
            var data = await _reportsService.GetAllSchoolDetails();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("SchoolDetails");

                // Add headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "School Name";
                worksheet.Cells[1, 3].Value = "SRN";
                worksheet.Cells[1, 4].Value = "Teacher First Name";
                worksheet.Cells[1, 5].Value = "Teacher Last Name";
                worksheet.Cells[1, 6].Value = "Teacher Email";
                worksheet.Cells[1, 7].Value = "Teacher Contact";
                worksheet.Cells[1, 8].Value = "Total Students";
                worksheet.Cells[1, 9].Value = "Total Payment";
                worksheet.Cells[1, 10].Value = "Total Payment Received";
                worksheet.Cells[1, 11].Value = "Payment Status";
                worksheet.Cells[1, 12].Value = "Academic Year";
                worksheet.Cells[1, 13].Value = "District";
                worksheet.Cells[1, 14].Value = "Class Name";
                worksheet.Cells[1, 15].Value = "Invoice";
                worksheet.Cells[1, 16].Value = "Receipt";
                worksheet.Cells[1, 17].Value = "Is Principal";
                worksheet.Cells[1, 18].Value = "Discount Percent";
                worksheet.Cells[1, 19].Value = "Discounted Price";

                // Add data rows
                for (var i = 0; i < data.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = data[i].Id;
                    worksheet.Cells[i + 2, 2].Value = data[i].SchoolName;
                    worksheet.Cells[i + 2, 3].Value = data[i].SRN;
                    worksheet.Cells[i + 2, 4].Value = data[i].TeacherFirstName;
                    worksheet.Cells[i + 2, 5].Value = data[i].TeacherLastName;
                    worksheet.Cells[i + 2, 6].Value = data[i].TeacherEmail;
                    worksheet.Cells[i + 2, 7].Value = data[i].TeacherContact;
                    worksheet.Cells[i + 2, 8].Value = data[i].TotalStudents;
                    worksheet.Cells[i + 2, 9].Value = data[i].TotalPayment;
                    worksheet.Cells[i + 2, 10].Value = data[i].TotalPaymentReceived;
                    worksheet.Cells[i + 2, 11].Value = data[i].PaymentStatus ? "Paid" : "Unpaid";
                    worksheet.Cells[i + 2, 12].Value = data[i].AcademicYear;
                    worksheet.Cells[i + 2, 13].Value = data[i].District;
                    worksheet.Cells[i + 2, 14].Value = data[i].ClassName;
                    worksheet.Cells[i + 2, 15].Value = data[i].Invoice;
                    worksheet.Cells[i + 2, 16].Value = data[i].Receipt;
                    worksheet.Cells[i + 2, 17].Value = data[i].IsPrincipal ? "Yes" : "No";
                    worksheet.Cells[i + 2, 18].Value = data[i].Discount_Percent;
                    worksheet.Cells[i + 2, 19].Value = data[i].Discounted_Price;
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                package.Save();
            }

            stream.Position = 0;

            // Return the Excel file
            var fileName = $"SchoolDetails-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

    }
}