using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Models.Ecommerce;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.Service
{
    public class SanPhamService : BaseService
    {
        public SanPhamService(ApplicationDbContext context) : base(context)
        {
        }
        public IEnumerable<SanPhamViewModels> SanPhamList(
            string? TenSP = null,
            int? MaLSP = null,
            decimal? DonGiaFrom = null,
            decimal? DonGiaTo = null,
            int[]? Variant = null)
        {
            var query = _context.SanPham.Include(x => x.ProductVariants).AsQueryable();

            if (!string.IsNullOrWhiteSpace(TenSP))
            {
                query = query.Where(x => x.TenSP.Contains(TenSP));
            }

            if (MaLSP.HasValue)
            {
                query = query.Where(x => x.IdLoaiSanPham == MaLSP.Value);
            }

            if (DonGiaFrom.HasValue)
            {
                query = query.Where(x => x.DonGia >= DonGiaFrom.Value);
            }

            if (DonGiaTo.HasValue)
            {
                query = query.Where(x => x.DonGia <= DonGiaTo.Value);
            }

            if (Variant != null && Variant.Length > 0)
            {
                query = query.Where(x => x.ProductVariants.Any(pv => Variant.Contains(pv.Id)));
            }

            var result = query
                .Select(x => new SanPhamViewModels
                {
                    Id = x.Id,
                    TenSP = x.TenSP,
                    LoaiSanPham = x.LoaiSanPham,
                    Anh = x.Anh,
                    DonGia = x.DonGia,
                    DVT = x.DVT,
                    MoTa = x.MoTa
                })
                .OrderByDescending(x => x.Id)
                .ToList();

            return result;
        }


        public SanPham GetSanPham(int id)
        {
            var sanPham = _context.SanPham
                .Include(x => x.LoaiSanPham)
                .Include(x => x.ProductVariants)
                    .ThenInclude(pv => pv.ProductVariantAttributes)
                        .ThenInclude(pva => pva.ProductAttribute)
                .Include(x => x.ProductVariants)
                    .ThenInclude(pv => pv.ProductVariantAttributes)
                        .ThenInclude(pva => pva.AttributeValue)
                .FirstOrDefault(x => x.Id == id);

            return sanPham;
        }


    }
}
