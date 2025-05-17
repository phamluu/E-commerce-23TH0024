using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext db;

        public BaseController(ApplicationDbContext context)
        {
            db = context;
        }
    }
}
