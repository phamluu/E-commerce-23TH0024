using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models.Ecommerce;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.Service
{
    public class DonHangService: BaseService
    {
        public DonHangService(ApplicationDbContext context) : base(context)
        {
        }
        public DonHang GetDonHang(int id)
        {
            DonHang donHang = _context.DonHangs.Include(x => x.ChiTietDonHangs).ThenInclude(d => d.SanPham)
                .Include(x => x.KhachHang)?.FirstOrDefault();
            return donHang;
        }

        public IEnumerable<DonHang> GetDonHangs()
        {
            var donHangs = _context.DonHangs.Include(x => x.KhachHang)
                .Include(x => x.ChiTietDonHangs).ThenInclude(d => d.SanPham).OrderByDescending(x => x.Id);
            return donHangs;
        }
    }
}
