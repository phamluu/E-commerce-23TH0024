using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly WorkManagementDbContext _workManagement;
        public NotificationController(ApplicationDbContext context, WorkManagementDbContext workManagement) : base(context) {
            _workManagement = workManagement;
        }
        [HttpGet]
        public IActionResult GetUnreadTasks()
        {
            
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var unreadTasks = _workManagement.Tasks
                .Include(x => x.Project)
                .Where(task => !_workManagement.TaskReads
                 .Any(tr => tr.IdTask == task.Id && tr.UserId == UserId))
                .OrderByDescending(t => t.CreatedAt)
                .Take(10)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    Project = x.Project.Name,
                    CreatedAt = x.CreatedAt
                })
                .ToList();

            return Json(unreadTasks);
        }
    }
}
