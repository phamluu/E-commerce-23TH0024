using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NhatKyXayDung.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BaseController : Controller
    {
        
    }
}
