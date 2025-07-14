using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using E_commerce_23TH0024.Models.Order;
using E_commerce_23TH0024.ViewModels;
using System.Linq;
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
        public DonHangViewModel GetDonHang(int id)
        {
            DonHang donHang = _context.DonHangs.Include(x => x.KhachHang).Include(x => x.Payments).Where(x => x.Id == id)?.FirstOrDefault();
            if (donHang == null) return null;
            
            var chiTietDonHangs = (from ct in _context.ChiTietDonHang
                                    join sp in _context.SanPham on ct.IdSanPham equals sp.Id
                                    where ct.IdDonHang == id
                                    select new DonHangDetailViewModel
                                    {
                                        Id = ct.Id,
                                        IdDonHang = ct.IdDonHang,
                                        IdSanPham = ct.IdSanPham,
                                        DonGia = ct.DonGia,
                                        SoLuong = ct.SoLuong,
                                        SanPham = sp
                                    }).ToList();
            var model = new DonHangViewModel
            {
                Id = donHang.Id,
                NgayDatHang = donHang.NgayDatHang,
                NgayGiaoHang = donHang.NgayGiaoHang,
                IdNhanVienDuyet = donHang.IdNhanVienDuyet,
                IdNhanVienGiao = donHang.IdNhanVienGiao,
                TinhTrang = donHang.TinhTrang,
                VAT = donHang.VAT,
                TotalAmount = donHang.TotalAmount,
                ShippingFee = donHang.ShippingFee,
                TotalProductAmount = donHang.TotalProductAmount,
                IdDiscountRule = donHang.IdDiscountRule,
                KhachHang = donHang.KhachHang,
                ChiTietDonHangVM = chiTietDonHangs,
                Payments = donHang.Payments
            };

            return model;
        }
       
        public IEnumerable<DonHangViewModel> GetDonHangs()
        {
            var donHangs = _context.DonHangs.Include(x => x.KhachHang)
                .Include(x => x.ChiTietDonHangs).Select(x => new DonHangViewModel
                {
                    Id = x.Id,
                    NgayDatHang = x.NgayDatHang,
                    NgayGiaoHang = x.NgayGiaoHang,
                    IdNhanVienGiao = x.IdNhanVienGiao,
                    IdNhanVienDuyet = x.IdNhanVienDuyet,
                    TinhTrang = x.TinhTrang,
                    KhachHang = x.KhachHang,
                    ChiTietDonHangs = x.ChiTietDonHangs
                }).OrderByDescending(x => x.Id);
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
