using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoaiSanPhams_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoaiSanPhams_23TH0024Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/LoaiSanPhams_23TH0024
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoaiSanPham.ToListAsync());
        }

        // GET: Admin/LoaiSanPhams_23TH0024/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var loaiSanPham = await _context.LoaiSanPham
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (loaiSanPham == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(loaiSanPham);
        //}

        // GET: Admin/LoaiSanPhams_23TH0024/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/LoaiSanPhams_23TH0024/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenLSP")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiSanPham);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm loại sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Không thành công!";
            return View(loaiSanPham);
        }

        // GET: Admin/LoaiSanPhams_23TH0024/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPham.FindAsync(id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }
            return View(loaiSanPham);
        }

        // POST: Admin/LoaiSanPhams_23TH0024/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenLSP")] LoaiSanPham loaiSanPham)
        {
            if (id != loaiSanPham.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiSanPham);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật loại sản phẩm thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiSanPhamExists(loaiSanPham.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Không cáp nhật thành cong!";
            return View(loaiSanPham);
        }

        // GET: Admin/LoaiSanPhams_23TH0024/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPham
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            return View(loaiSanPham);
        }

        // POST: Admin/LoaiSanPhams_23TH0024/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiSanPham = await _context.LoaiSanPham.FindAsync(id);
            if (loaiSanPham != null)
            {
                _context.LoaiSanPham.Remove(loaiSanPham);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tồn tại loại sản phẩm!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiSanPhamExists(int id)
        {
            return _context.LoaiSanPham.Any(e => e.Id == id);
        }
    }
}
