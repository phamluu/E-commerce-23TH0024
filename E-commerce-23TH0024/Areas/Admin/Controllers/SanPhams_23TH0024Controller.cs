using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Models.Ecommerce;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhams_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public SanPhams_23TH0024Controller(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        private IEnumerable<SanPhamViewModels> GetProducts(string TenSP = null, int? MaLSP = null, decimal? DonGiaFrom = null,
            decimal? DonGiaTo = null, int[] Variant = null)
        {
            string VariantStr = null;
            if (Variant != null)
            {
                VariantStr = string.Join(",", Variant);
            }
            var product = _context.SanPham
                //_context.SanPham.FromSqlRaw("EXEC SanPham_TimKiem @TenSP, @MaLSP, @DonGiaFrom, @DonGiaTo, @Variant",
                //new SqlParameter("@TenSP", (object)TenSP ?? DBNull.Value),
                //new SqlParameter("@MaLSP", (object)MaLSP ?? DBNull.Value),
                //new SqlParameter("@DonGiaFrom", (object)DonGiaFrom ?? DBNull.Value),
                //new SqlParameter("@DonGiaTo", (object)DonGiaTo ?? DBNull.Value),
                //new SqlParameter("@Variant", (object)VariantStr ?? DBNull.Value)
                //)
                .Select(x => new SanPhamViewModels
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


        [HttpGet]
        public ActionResult TimKiemNC(string TenSP = null, int? IdLoaiSanPham = null, decimal? DonGiaFrom = null, decimal? DonGiaTo = null)
        {
            ViewBag.IdLoaiSanPham = new SelectList(_context.LoaiSanPham, "Id", "TenLSP");
            ViewBag.TenSP = TenSP;
            ViewBag.DonGiaFrom = DonGiaFrom;
            ViewBag.DonGiaTo = DonGiaTo;
            var products = GetProducts(TenSP, IdLoaiSanPham, DonGiaFrom, DonGiaTo).ToList();

            if (products.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";
            return View("Index", products);
        }
        

        public ActionResult Index()
        {
            var sanPhams = GetProducts();
            ViewBag.IdLoaiSanPham = new SelectList(_context.LoaiSanPham, "Id", "TenLSP");
            return View(sanPhams.ToList());
        }
        

        public ActionResult Create()
        {
            ViewBag.IdLoaiSanPham = new SelectList(_context.LoaiSanPham, "Id", "TenLSP");
            SanPham sanPham = new SanPham();
            return View(sanPham);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,IdLoaiSanPham,TenSP,MoTa,DonGia,DVT,Anh")] SanPham sanPham, IFormFile Avatar = null)
        {

            if (ModelState.IsValid)
            {
                if (Avatar != null && Avatar.Length > 0)
                {
                    var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    var fileName = Path.GetFileName(Avatar.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Avatar.CopyToAsync(fileStream);
                    }
                    sanPham.Anh = fileName;

                }
                _context.SanPham.Add(sanPham);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Không thể thêm sản phẩm";
            ViewBag.IdLoaiSanPham = new SelectList(_context.LoaiSanPham, "Id", "TenLSP", sanPham.IdLoaiSanPham);
            return View(sanPham);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            SanPham sanPham = _context.SanPham.Find(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewBag.IdLoaiSanPham = new SelectList(_context.LoaiSanPham, "Id", "TenLSP", sanPham.IdLoaiSanPham);
            return View(sanPham);
        }
        

        public void XoaAnhCu(string postedFileName)
        {
            if (string.IsNullOrEmpty(postedFileName)) return;
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
            string filePath = Path.Combine(uploadsFolder, postedFileName);
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi xóa ảnh: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("File không tồn tại: " + filePath);
            }
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,IdLoaiSanPham,TenSP,MoTa,DonGia,DVT,Anh")] SanPham sanPham, IFormFile Avatar = null)
        {
            if (ModelState.IsValid)
            {
                if (Avatar != null)
                {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                        string uniqueFileName = Path.GetFileName(Avatar.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await Avatar.CopyToAsync(fileStream);
                        }

                        XoaAnhCu(sanPham.Anh);
                        sanPham.Anh = uniqueFileName;
                    
                }

                _context.Entry(sanPham).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Không thể cập nhật sản phẩm";
            ViewBag.IdLoaiSanPham = new SelectList(_context.LoaiSanPham, "Id", "TenLSP", sanPham.IdLoaiSanPham);
            return View(sanPham);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            SanPham sanPham = _context.SanPham.Find(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            return View(sanPham);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = _context.SanPham.Find(id);
            if (sanPham == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm để xóa!";
                return RedirectToAction("Index");
            }
            XoaAnhCu(sanPham.Anh);
            _context.SanPham.Remove(sanPham);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            return RedirectToAction("Index");
        }
        
        public ActionResult DeleteAll()
        {
            IEnumerable<SanPham> sanPham = _context.SanPham;
            if (sanPham != null)
            {
                foreach (var item in sanPham)
                {
                    _context.SanPham.Remove(item);
                    XoaAnhCu(item.Anh);
                }
                TempData["SuccessMessage"] = "Tất cả sản phẩm đã được xóa thành công!";
                _context.SaveChanges();
            }
            else
            {
                TempData["ErrorMessage"] = "Không có sản phẩm để xóa!";
            }
            return RedirectToAction("Index");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
