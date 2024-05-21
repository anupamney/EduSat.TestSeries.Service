using EduSat.TestSeries.Service.Models.DTOs.Request;
using EduSat.TestSeries.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSat.TestSeries.Service.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {

        private readonly ISchoolsService _schoolsService;
        public PaymentController(ISchoolsService schoolsService)
        {
            _schoolsService = schoolsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            var data = await _schoolsService.GetPayments();
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> AddPayment(Payment p)
        {
            var data = await _schoolsService.AddPayment(p);
            return Ok(data);
        }
    }
}
