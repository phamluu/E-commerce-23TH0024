using E_commerce_23TH0024.Models.Order;
using E_commerce_23TH0024.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_23TH0024.ViewModels
{
    public class UserViewModel: IdentityUser
    {
        public List<string> Roles { get; set; }
        public bool IsDuyetHoSo { get; set; }
        public KhachHang KhachHang { get; set; }
        public NhanVien NhanVien { get; set; }
    }
}
