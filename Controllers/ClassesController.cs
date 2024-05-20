using EduSat.TestSeries.Service.Models.DTOs.Request;
using EduSat.TestSeries.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSat.TestSeries.Service.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : Controller
    {
        private readonly ISchoolsService _schoolsService;
        public ClassesController(ISchoolsService schoolsService)
        {
            _schoolsService = schoolsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            var data = await _schoolsService.GetClasses();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> PostClass(Class clas)
        {
            var data = await _schoolsService.AddClass(clas);
            return Ok(data);
        }
        
    }
}
