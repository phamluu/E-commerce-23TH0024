using E_commerce_23TH0024.Models.Identity;
using E_commerce_23TH0024.Models.Ecommerce;

namespace E_commerce_23TH0024.Models
{
    public class KhachHangViewModel:KhachHang
    {
        public ApplicationUser AspNetUser { get; set; }
    }
}
