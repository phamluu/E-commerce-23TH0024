using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.ViewComponents
{
    public class LoaiSanPhamViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public LoaiSanPhamViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewname = "LoaiSanPham")
        {
            var lsp = await _context.LoaiSanPham.ToListAsync();
            var lspViewModels = lsp.Select(sp => new LoaiSanPhamViewModels
            {
                Id = sp.Id,
                TenLSP = sp.TenLSP,
            }).ToList();
            return View(viewname, lspViewModels);
        }
    }
}
