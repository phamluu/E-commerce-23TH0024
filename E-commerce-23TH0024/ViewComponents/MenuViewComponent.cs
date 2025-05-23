using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly MenuService _service;

        public MenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
            _service = new MenuService(context);
        }

        public async Task<IViewComponentResult> InvokeAsync(string IdNhomMenu, string viewname)
        {
            if (IdNhomMenu == null)
                return Content("Menu ID is null");

            var menu = _service.GetMenus(IdNhomMenu);
            return View(viewname, menu);
        }
    }
}
