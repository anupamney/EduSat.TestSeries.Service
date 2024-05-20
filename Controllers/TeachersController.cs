using EduSat.TestSeries.Service.Models.DTOs.Request;
using EduSat.TestSeries.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSat.TestSeries.Service.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]

    public class TeachersController : Controller
    {
        private readonly ISchoolsService _schoolsService;
        public TeachersController(ISchoolsService schoolsService)
        {
            _schoolsService = schoolsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            var data = await _schoolsService.GetTeachers();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> PostTeacher(Teacher teacher)
        {
            var data = await _schoolsService.AddTeacher(teacher);
            return Ok(data);
        }
    }
}
