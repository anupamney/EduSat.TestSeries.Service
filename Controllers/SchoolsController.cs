using EduSat.TestSeries.Service.Models;
using EduSat.TestSeries.Service.Models.DTOs.Request;
using EduSat.TestSeries.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduSat.TestSeries.Service.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolsController : Controller
    {
        private readonly ISchoolsService _schoolsService;
        public SchoolsController(ISchoolsService schoolsService)
        {
            _schoolsService = schoolsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchool()
        {
            var data = await _schoolsService.GetSchools();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> PostSchool(School school)
        {
            var data = await _schoolsService.PostSchool(school);
            return Ok(data);
        }
    }
}
