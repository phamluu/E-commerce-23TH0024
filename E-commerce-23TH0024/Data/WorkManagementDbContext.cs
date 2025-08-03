using Humanizer;
using Microsoft.EntityFrameworkCore;
using WorkManagement.Data;

namespace E_commerce_23TH0024.Data
{
    public class WorkManagementDbContext:WorkDbContext
    {
        //public WorkManagementDbContext(DbContextOptions<WorkDbContext> options)
        //: base(options)
        //{
        //}

        public WorkManagementDbContext(DbContextOptions<WorkManagementDbContext> options)
    : base(options)
        {
        }

    }
}
