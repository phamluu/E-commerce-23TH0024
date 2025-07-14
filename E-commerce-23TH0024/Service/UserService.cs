using AutoMapper;
using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Lib.Enums;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Models.Order;
using E_commerce_23TH0024.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.Service
{
    public class UserService: BaseService
    {
        private readonly IMapper _mapper;
        public UserService(ApplicationDbContext context) : base(context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<KhachHang, KhachHangViewModel>();
            });
            _mapper = config.CreateMapper();
        }

        #region Nhân viên
        public IEnumerable<NhanVien> GetNhanViens()
        {
            var nhanviens = _context.NhanVien.Include(x => x.AspNetUser).OrderByDescending(x => x.Id);
            return nhanviens;
        }

        public NhanVien GetNhanVien(int id)
        {
            var nhanVien = _context.NhanVien.Include(x => x.AspNetUser).Where(x => x.Id == id).FirstOrDefault();
            return nhanVien;
        }

        public bool UpdateNhanVien(NhanVien nhanVien)
        {
            var existingNhanVien = _context.NhanVien.Find(nhanVien.Id);
            if (existingNhanVien == null)
            {
                return false;
            }
            existingNhanVien.HoTen = nhanVien.HoTen;
            existingNhanVien.SoDienThoai = nhanVien.SoDienThoai;
            existingNhanVien.DiaChi = nhanVien.DiaChi;
            _context.SaveChanges();
            return true;
        }
        public (CheckNhanVienStatus Status, string) CheckNhanVien(string Email)
        {
            var result = new List<Tuple<int, string>>();
            var user = _context.Users.Where(x => x.Email == Email).FirstOrDefault();
            if (user != null)
            {
                var nhanVien = _context.NhanVien.Where(x => x.IdAspNetUsers == user.Id).FirstOrDefault();
                if (nhanVien != null)
                {
                    return (CheckNhanVienStatus.LaNhanVien, "Là nhân viên");
                }
                return (CheckNhanVienStatus.CoTaiKhoanChuaPhaiNhanVien, "Đã có tài khoản. Chưa phải là nhân viên");
            }
            return (CheckNhanVienStatus.ChuaCoTaiKhoan, "Chưa có tài khoản người dùng");
        }
        #endregion

        #region Khách hàng
        public IEnumerable<KhachHangViewModel> GetKhachHangs()
        {
            var khachHangs = _context.KhachHang.Include(x => x.CustomerType).OrderByDescending(x => x.Id).ToList();

            var userIds = khachHangs
                .Where(k => !string.IsNullOrEmpty(k.IdAspNetUsers))
                .Select(k => k.IdAspNetUsers)
                .Distinct()
                .ToList();

            var users = _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id);

            var result = khachHangs.Select(kh =>
            {
                var vm = _mapper.Map<KhachHangViewModel>(kh);

                if (!string.IsNullOrEmpty(kh.IdAspNetUsers) && users.TryGetValue(kh.IdAspNetUsers, out var user))
                {
                    vm.AspNetUser = user;
                }

                return vm;
            });

            return result;
        }

        #endregion
    }
}
