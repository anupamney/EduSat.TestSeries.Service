using EduSat.TestSeries.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSat.TestSeries.Service.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]

    public class ReportsController : Controller
    {
        private readonly IReportsService _reportsService;
        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSchoolDetails()
        {
            var data = await _reportsService.GetAllSchoolDetails();
            return Ok(data);
        }
    }
}
