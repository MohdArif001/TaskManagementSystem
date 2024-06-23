using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskAssignmentAndNotificationService.Models;
using TaskAssignmentAndNotificationService.Repository.Interfaces;

namespace TaskAssignmentAndNotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentAndNotificationController : ControllerBase
    {
        private readonly IAssignmentAndNotification _assignmentAndNotification;
        public AssignmentAndNotificationController(IAssignmentAndNotification  assignmentAndNotification)
        {
            _assignmentAndNotification = assignmentAndNotification;
        }
        [Route("SentNotification")]
        [Authorize]
        [HttpPost]
        public ActionResult SentNotification(TaskAssign taskAssign)
        {
            bool isTaskAssign = _assignmentAndNotification.SentNotification(taskAssign);
            if (isTaskAssign)
            {
                return Ok(new
                {
                    Message = "Notification Sent Successfully",
                });
            }
            else {
                return Ok(new
                {
                    Message = "Notification not sent Successfully",
                }); 
            }       
        }

    }
}
