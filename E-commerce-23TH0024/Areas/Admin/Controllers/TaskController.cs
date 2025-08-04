using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkManagement.Entities;
using WorkManagement.Services;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    public class TaskController : BaseController
    {
        private readonly WorkManagementDbContext _workManagement;
        private readonly ProjectService _projectService;
        private readonly IHubContext<TaskHub> _hubContext;
        public TaskController(
                ApplicationDbContext context,
                ProjectService projectService,
                WorkManagementDbContext workManagement,
                IHubContext<TaskHub> hubContext
            ) :base(context) {
            _projectService = projectService;
            _workManagement = workManagement;
            _hubContext = hubContext;
        }
        // GET: TaskController
        public ActionResult Index()
        {
            var model = _projectService.GetTaskItems();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder([FromBody] List<int> orderedIds)
        {
            var tasks = await _workManagement.Tasks
                .Where(t => orderedIds.Contains(t.Id))
                .ToListAsync();

            for (int i = 0; i < orderedIds.Count; i++)
            {
                var task = tasks.FirstOrDefault(t => t.Id == orderedIds[i]);
                if (task != null)
                {
                    task.SortOrder = i;
                }
            }

            await _workManagement.SaveChangesAsync();
            return Ok();
        }


        // GET: TaskController/Details/5
        public ActionResult Details(int id)
        {
            var model = _workManagement.Tasks.Include(x => x.Project).FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            string IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var taskRead = _workManagement.TaskReads.FirstOrDefault(x => x.IdTask == id && x.UserId == IdUser);
            if (taskRead == null)
            {
                _workManagement.TaskReads.Add(new TaskRead { IdTask = id, UserId = IdUser, ReadAt = DateTime.Now });
                _workManagement.SaveChanges();
            }
            return View(model);
        }

        // GET: TaskController/Create
        public ActionResult Create()
        {
            ViewBag.IdProject = new SelectList(_workManagement.Projects, "Id", "Name");
            return View();
        }

        // POST: TaskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(TaskItem model)
        {
            try
            {
                _workManagement.Tasks.Add(model);
                _workManagement.SaveChanges();
                TempData["SuccessMessage"] = "Thêm task thành công";

                var task = _workManagement.Tasks.Include(x => x.Project).FirstOrDefault(x => x.Id == model.Id);

                if (task != null)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Project = task.Project?.Name
                    });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Không thể thêm task";
                return View(model);
            }
        }

        // GET: TaskController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _workManagement.Tasks.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewBag.IdProject = new SelectList(_workManagement.Projects, "Id", "Name", model.IdProject);
            return View(model);
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaskItem model)
        {
            try
            {
                var task = _workManagement.Tasks.Find(model.Id);
                if (task == null)
                {
                    TempData["ErrorMessage"] = "Task không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }
                task.Title = model.Title;
                task.Description = model.Description;
                task.Status = model.Status;
                task.IdProject = model.IdProject;
                _workManagement.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật task thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Không thể cập nhật task";
                return View(model);
            }
        }


        // POST: TaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                _workManagement.Tasks.Remove(_workManagement.Tasks.Find(id));
                _workManagement.SaveChanges();
                TempData["SuccessMessage"] = "Xóa task thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Không thể xóa task";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
