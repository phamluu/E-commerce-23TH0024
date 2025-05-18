using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.IO.Compression;
using System.Reflection;
using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using E_commerce_23TH0024.Models.Ecommerce;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class FileTransfer_23TH0024Controller : BaseController
    {
        public FileTransfer_23TH0024Controller(ApplicationDbContext context) : base(context)
        {
        }

        #region ImportExcel
        public ActionResult ImportExcel()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ImportExcelAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Please select a file to upload.";
                return View();
            }

            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataTable dt = ReadExcelFile(filePath);
                SaveDataToDatabase(dt);
                ViewBag.Message = "File imported successfully!";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                return View();
            }
        }

        public DataTable ReadExcelFile(string filePath, int wordsheet = 0)
        {
            DataTable dt = new DataTable();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[wordsheet];
                bool hasHeader = true;
                foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                {
                    dt.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                for (int rowNum = 2; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                {
                    var row = dt.NewRow();
                    for (int colNum = 1; colNum <= worksheet.Dimension.End.Column; colNum++)
                    {
                        row[colNum - 1] = worksheet.Cells[rowNum, colNum].Text;
                    }
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        private void SaveDataToDatabase(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                var product = new SanPham
                {
                    TenSP = row["TenSP"].ToString(),
                    DonGia = Convert.ToDecimal(row["DonGia"]),
                    DVT = row["DVT"].ToString(),
                    IdLoaiSanPham = Convert.ToInt32(row["IdLoaiSanPham"]),
                    Anh = row["Anh"].ToString(),
                    MoTa = row["MoTa"].ToString(),
                };
                db.SanPham.Add(product);
            }
            db.SaveChanges();
        }
        #endregion

        #region ImportExcel tùy chỉnh
        public ActionResult ImportExcel_Custom()
        {
            ViewBag.MaLSP = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ImportExcel_CustomAsync(IFormFile file, int? MaLSP, decimal? DonGia, string TenSP, string DVT)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Chọn tệp nén file hình.";
                ModelState.AddModelError("file", "Chọn tệp nén chứa hình ảnh");
                ViewBag.TenSP = TenSP;
                ViewBag.DonGia = DonGia;
                ViewBag.DVT = DVT;
                ViewBag.MaLSP = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP");
                return View();
            }

            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uploadPath = Path.Combine(uploadsFolder, file.FileName);
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                //var uploadPath = Server.MapPath("~/Uploads");
                //var zipFilePath = Path.Combine(uploadPath, file.FileName);
                //file.SaveAs(zipFilePath);

                // Đường dẫn thư mục giải nén
                var extractPath = Path.Combine(uploadPath, Path.GetFileNameWithoutExtension(file.FileName));

                using (var zipStream = new FileStream(uploadPath, FileMode.Open, FileAccess.Read))

                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    foreach (var entry in archive.Entries)
                    {
                        var filePath = Path.Combine(extractPath, entry.FullName);
                        // Đảm bảo thư mục đích tồn tại trước khi giải nén
                        var directoryPath = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        if (entry.FullName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                            entry.FullName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                            entry.FullName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                // Mở một Stream để giải nén tệp
                                using (var entryStream = entry.Open())
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    entryStream.CopyTo(fileStream);
                                    SanPham sp = new SanPham();
                                    sp.IdLoaiSanPham = MaLSP;
                                    sp.DonGia = DonGia;
                                    sp.DVT = DVT;
                                    sp.Anh = Path.GetFileNameWithoutExtension(file.FileName) + "/" + entry.FullName;
                                    if (!string.IsNullOrEmpty(TenSP))
                                    {
                                        sp.TenSP = TenSP + " " + Path.GetFileNameWithoutExtension(entry.FullName);
                                    }
                                    db.SanPham.Add(sp);
                                    db.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                // Hiển thị lỗi nếu không thể giải nén
                                ViewBag.Message = "Lỗi khi giải nén tệp: " + ex.Message;
                                ViewBag.TenSP = TenSP;
                                ViewBag.DonGia = DonGia;
                                ViewBag.DVT = DVT;
                                ViewBag.MaLSP = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP");
                                return View();
                            }

                        }
                    }
                }

                ViewBag.Message = "File imported successfully!";
                ViewBag.TenSP = TenSP;
                ViewBag.DonGia = DonGia;
                ViewBag.DVT = DVT;
                ViewBag.MaLSP = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP");
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                ViewBag.TenSP = TenSP;
                ViewBag.DonGia = DonGia;
                ViewBag.DVT = DVT;
                ViewBag.MaLSP = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP");
                return View();
            }
        }
        #endregion

        #region ExportExcel
        public ActionResult ExportExcel()
        {
            var products = db.SanPham.ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // Thêm một worksheet
                var worksheet = package.Workbook.Worksheets.Add("Products");

                // Thêm tiêu đề cột
                worksheet.Cells[1, 1].Value = "MaSP";
                worksheet.Cells[1, 2].Value = "TenSP";
                worksheet.Cells[1, 3].Value = "DonGia";
                worksheet.Cells[1, 4].Value = "DVT";
                worksheet.Cells[1, 5].Value = "MaLSP";
                worksheet.Cells[1, 6].Value = "Anh";
                worksheet.Cells[1, 7].Value = "MoTa";

                // Thêm dữ liệu vào worksheet
                int row = 2;
                foreach (var product in products)
                {
                    worksheet.Cells[row, 1].Value = product.Id;
                    worksheet.Cells[row, 2].Value = product.TenSP;
                    worksheet.Cells[row, 3].Value = product.DonGia;
                    worksheet.Cells[row, 4].Value = product.DVT;
                    worksheet.Cells[row, 5].Value = product.IdLoaiSanPham;
                    worksheet.Cells[row, 6].Value = product.Anh;
                    worksheet.Cells[row, 7].Value = product.MoTa;
                    row++;
                }

                // Lưu file Excel vào bộ nhớ
                var memoryStream = new MemoryStream();
                package.SaveAs(memoryStream);

                // Đặt con trỏ bộ nhớ về đầu để tải xuống
                memoryStream.Position = 0;

                // Trả về file Excel cho người dùng tải về
                string fileName = "Products_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion
    }
}