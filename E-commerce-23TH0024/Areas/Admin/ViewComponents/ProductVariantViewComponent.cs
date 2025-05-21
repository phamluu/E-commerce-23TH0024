using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.Areas.Admin.ViewComponents
{
    public class ProductVariantViewComponent : ViewComponent // Corrected 'ViewComponents' to 'ViewComponent'  
    {
        private readonly ApplicationDbContext db;
        public ProductVariantViewComponent(ApplicationDbContext context)
        {
            db = context;
        }

        // Add a method to render the view (optional, based on your requirements)  
        public async Task<IViewComponentResult> InvokeAsync(string viewname)
        {
            switch (viewname)
            {
                case "Create":
                    var attributes = await db.ProductAttributes.Include(a => a.AttributeValues).ToListAsync();
                    return View("Create", attributes);

                default:
                    return Content("Invalid viewname.");
            }
        }
    }
}
