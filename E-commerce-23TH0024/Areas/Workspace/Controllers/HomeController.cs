using Microsoft.AspNetCore.Mvc;

namespace E_commerce_23TH0024.Areas.Workspace.Controllers
{
    public class HomeController : BaseWorkspaceController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
