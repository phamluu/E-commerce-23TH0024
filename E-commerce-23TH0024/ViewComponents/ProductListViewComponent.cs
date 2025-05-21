using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.ViewComponents
{
    public class ProductListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ProductListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewname)
        {
            switch (viewname)
            {
                case "ProductList":
                    var products = await _context.SanPham.Include(sp => sp.LoaiSanPham).ToListAsync();
                    var productViewModels = products.Select(sp => new SanPhamViewModels
                    {
                        Id = sp.Id,
                        TenSP = sp.TenSP,
                        DonGia = sp.DonGia,
                        MoTa = sp.MoTa,
                        Anh = sp.Anh,
                        IdLoaiSanPham = sp.IdLoaiSanPham,
                        DVT = sp.DVT,
                        LoaiSanPham = sp.LoaiSanPham,
                        ProductVariants = sp.ProductVariants,
                    }).ToList();
                    var attribute = _context.ProductAttributes;
                    ViewBag.Attribute = attribute;
                    return View(viewname, productViewModels);
                    
                default:
                    viewname = "ProductAttribute";
                    var attributes = await _context.ProductAttributes.Include(x => x.AttributeValues).ToListAsync();
                    return View(viewname, attributes);
            }
            
        }
    }
}
