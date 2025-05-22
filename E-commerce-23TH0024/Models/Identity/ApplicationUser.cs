using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_23TH0024.Models.Identity
{
    public class ApplicationUser: IdentityUser
    {
        //public virtual ICollection<KhachHang>? KhachHang { get; set; }
        public virtual NhanVien? NhanVien { get; set; }
    }
}
