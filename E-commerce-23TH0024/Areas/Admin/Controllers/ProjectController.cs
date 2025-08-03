using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkManagement.Services;
using WorkManagement.Entities;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    public class ProjectController : BaseController
    {
        protected readonly ApplicationDbContext db;
        private readonly WorkManagementDbContext _workManagement;
        private readonly ProjectService _projectService;
        public ProjectController(ApplicationDbContext context, 
            ProjectService projectService,
            WorkManagementDbContext workManagement):base(context)
        {
            db = context;
            _workManagement = workManagement;
            _projectService = projectService;
        }
        public ActionResult Index()
        {
           var model = _projectService.GetProjects();
            return View(model);
        }

        // GET: ProjectController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProjectController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project model)
        {
            try
            {
                model.CreatedAt = DateTime.Now;
                _workManagement.Projects.Add(model);
                _workManagement.SaveChanges();
                TempData["SuccessMessage"] = "Thêm dự án thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Không thể thêm dự án";
                return View(model);
            }
        }

        // GET: ProjectController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _workManagement.Projects.Find(id);
            return View(model);
        }

        // POST: ProjectController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project model)
        {
            try
            {
                var existingProject = _workManagement.Projects.Find(model.Id);
                if (existingProject == null)
                {
                    TempData["ErrorMessage"] = "Dự án không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }
                existingProject.Name = model.Name;
                existingProject.Description = model.Description;
                existingProject.StartDate = model.StartDate;
                existingProject.EndDate = model.EndDate;
                existingProject.ProjectPrice = model.ProjectPrice;

                _workManagement.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật dự án thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Không thể cập nhật dự án";
                return View(model);
            }
        }


        // POST: ProjectController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                _workManagement.Projects.Remove(_workManagement.Projects.Find(id));
                _workManagement.SaveChanges();
                TempData["SuccessMessage"] = "Xóa dự án thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Không thể xóa dự án";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
