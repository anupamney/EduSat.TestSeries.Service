using EduSat.TestSeries.Service.Models.DTOs.Request.Notification;
using EduSat.TestSeries.Service.Models.DTOs.Response;
using EduSat.TestSeries.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EduSat.TestSeries.Service.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpPost]
        public async Task<IActionResult> NotifyTeachersAsync([FromForm] NotificationRequest notificationRequest)
        {
            var recipients = JsonConvert.DeserializeObject<SchoolDetails[]>(notificationRequest.Recipients);

            if (recipients == null)
            {
                return BadRequest("Invalid recipients format.");
            }
            return Ok(await _notificationService.Notify(notificationRequest,recipients));
        }
    }
}
