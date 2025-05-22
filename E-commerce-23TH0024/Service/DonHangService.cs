using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Models.Ecommerce;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
namespace E_commerce_23TH0024.Service
{
    public class DonHangService: BaseService
    {
        private readonly IMapper _mapper;
        public DonHangService(ApplicationDbContext context) : base(context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DonHang, DonHangViewModel>();
            });
            _mapper = config.CreateMapper();
        }
        public DonHang GetDonHang(int id)
        {
            DonHang donHang = _context.DonHangs.Include(x => x.ChiTietDonHangs).ThenInclude(d => d.SanPham)
                .Include(x => x.KhachHang)?.FirstOrDefault();
            return donHang;
        }
        public DonHangViewModel GetDonHangViewModel(int id)
        {
            DonHang donHang = _context.DonHangs.Include(x => x.ChiTietDonHangs).ThenInclude(d => d.SanPham)
                .Include(x => x.KhachHang)?.FirstOrDefault();
            if (donHang != null)
            {
                var rs = _mapper.Map<DonHangViewModel>(donHang);
                rs.NhanVienGiao = _context.NhanVien.FirstOrDefault(x => x.Id == donHang.IdNhanVienGiao);
                rs.NhanVienDuyet = _context.NhanVien.FirstOrDefault(x => x.Id == donHang.IdNhanVienDuyet);
                return rs;
            }

            return null;
        }
        public IEnumerable<DonHang> GetDonHangs()
        {
            var donHangs = _context.DonHangs.Include(x => x.KhachHang)
                .Include(x => x.ChiTietDonHangs).ThenInclude(d => d.SanPham).OrderByDescending(x => x.Id);
            return donHangs;
        }
    
        public bool UpdateDonHang(DonHang donHang)
        {
            var existingDonHang = _context.DonHangs.Find(donHang.Id);
            if (existingDonHang == null)
            {
                return false;
            }
            existingDonHang.NgayGiaoHang = donHang.NgayGiaoHang;
            existingDonHang.IdNhanVienGiao = donHang.IdNhanVienGiao;
            existingDonHang.TinhTrang = donHang.TinhTrang;
            _context.SaveChanges();
            return true;
        }
    }
}
