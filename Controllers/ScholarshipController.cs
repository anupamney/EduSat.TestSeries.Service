using EduSat.TestSeries.Service.Models.DTOs.Request;
using EduSat.TestSeries.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSat.TestSeries.Service.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class ScholarshipController : Controller
    {

        private readonly ISchoolsService _schoolsService;
        public ScholarshipController(ISchoolsService schoolsService)
        {
            _schoolsService = schoolsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetScholarship()
        {
            var data = await _schoolsService.GetScholarships();
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> AddScholarship(Scholarship s)
        {
            var data = await _schoolsService.AddScholarship(s);
            return Ok(data);
        }
    }
}
