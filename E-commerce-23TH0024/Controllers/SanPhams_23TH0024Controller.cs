using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace E_commerce_23TH0024.Controllers
{
    public class SanPhams_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SanPhamService _service;

        public SanPhams_23TH0024Controller(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _service = new SanPhamService(context);
        }
        private IEnumerable<SanPhamViewModels> GetProducts(string TenSP = null, int? MaLSP = null, decimal? DonGiaFrom = null,
            decimal? DonGiaTo = null, int[] Variant = null)
        {
            string VariantStr = null;
            if (Variant != null)
            {
                VariantStr = string.Join(",", Variant);
            }
            var product = _context.SanPham.FromSqlRaw("EXEC SanPham_TimKiem @TenSP, @MaLSP, @DonGiaFrom, @DonGiaTo, @Variant",
                new SqlParameter("@TenSP", (object)TenSP ?? DBNull.Value),
                new SqlParameter("@MaLSP", (object)MaLSP ?? DBNull.Value),
                new SqlParameter("@DonGiaFrom", (object)DonGiaFrom ?? DBNull.Value),
                new SqlParameter("@DonGiaTo", (object)DonGiaTo ?? DBNull.Value),
                new SqlParameter("@Variant", (object)VariantStr ?? DBNull.Value)
                ).Select(x => new SanPhamViewModels
                {
                    Id = x.Id,
                    TenSP = x.TenSP,
                    LoaiSanPham = x.LoaiSanPham,
                    Anh = x.Anh,
                    DonGia = x.DonGia,
                    DVT = x.DVT,
                    MoTa = x.MoTa,
                }).OrderByDescending(x => x.Id);
            return product;
        }

        //public async Task<IViewComponentResult> GetProductsPartial(string viewname)
        //{
        //    var sanPhams = GetProducts().ToList();
        //    var attribute = _context.ProductAttributes;
        //    ViewBag.Attribute = attribute;
        //    return View(viewname, sanPhams);
        //}
        public ActionResult ProductListForCategory(int id)
        {
            //var sanPham = GetProducts("", id).ToList();

            var loaiSPEntity = _context.LoaiSanPham.SingleOrDefault(x => x.Id == id);
            if (loaiSPEntity != null)
            {
                LoaiSanPhamViewModels loaiSanPham = new LoaiSanPhamViewModels();
                loaiSanPham.TenLSP = loaiSPEntity.TenLSP;
                loaiSanPham.Id = loaiSPEntity.Id;
                ViewBag.LoaiSanPham = loaiSanPham;
            }
            var attribute = _context.ProductAttributes;
            ViewBag.Attribute = attribute;

            var sanPhams = _context.SanPham.Where(x => x.IdLoaiSanPham == id)
                .Select(x => new SanPhamViewModels { 
                    Id = x.Id, 
                    TenSP = x.TenSP, 
                    Anh = x.Anh, 
                    DonGia = x.DonGia, 
                    DVT = x.DVT, 
                    MoTa = x.MoTa,
                    LoaiSanPham = x.LoaiSanPham
                }).ToList();
            return View("TimKiemNC1", sanPhams);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            //SanPham sanPham = _context.SanPham.Find(id);
            SanPham sanPham = _context.SanPham
            .Include(s => s.LoaiSanPham)
            .FirstOrDefault(s => s.Id == id);
            var sanPhamViewModel = ObjectMapper.Map<SanPham, SanPhamViewModels>(sanPham);
            if (sanPham == null)
            {
                return NotFound();
            }
            return View(sanPhamViewModel);
        }

        [HttpGet]
        public ActionResult TimKiemNC1(string TenSP = null, int? MaLSP = null, decimal? DonGiaFrom = null, decimal? DonGiaTo = null, int[] Variant = null)
        {
            ViewBag.MaLSP = new SelectList(_context.LoaiSanPham, "Id", "TenLSP");
            ViewBag.TenSP = TenSP;
            ViewBag.DonGiaFrom = DonGiaFrom;
            ViewBag.DonGiaTo = DonGiaTo;
            ViewBag.Variant = Variant;
            string VariantStr = Variant != null ? string.Join(",", Variant) : "";
            //var product = GetProducts(TenSP, MaLSP, DonGiaFrom, DonGiaTo, Variant).ToList();
            var product = _service.SanPhamList(TenSP, MaLSP, DonGiaFrom, DonGiaTo, Variant);
            if (product.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";
            var attribute = _context.ProductAttributes;
            ViewBag.Attribute = attribute;
            return View("TimKiemNC1", product);
        }

    }
}
