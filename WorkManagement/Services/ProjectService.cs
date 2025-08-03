using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkManagement.Data;
using WorkManagement.Entities;

namespace WorkManagement.Services
{
    public class ProjectService
    {
        private readonly WorkDbContext _context;
        public ProjectService(WorkDbContext context)
        {
            _context = context;
        }
        #region Project
            public IEnumerable<Project> GetProjects()
            {
                var model = _context.Projects.OrderByDescending(x => x.CreatedAt).ToList();
                return model;
            }
        #endregion

        #region Task
            public IEnumerable<TaskItem> GetTaskItems()
            {
                var model = _context.Tasks.Include(x => x.Project).OrderByDescending(x => x.CreatedAt).ToList();
                return model;
            }
            public IEnumerable<TaskItem> GetTasksForProject(int IdProject)
            {
                var model = _context.Tasks.Include(x => x.Project).Where(x => x.IdProject == IdProject).ToList();
                return model;
            }
        #endregion

    }
}
