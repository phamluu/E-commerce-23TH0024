using E_commerce_23TH0024.Models.Identity;
using E_commerce_23TH0024.Models.Users;

namespace E_commerce_23TH0024.Models
{
    public class NhanVienViewModel
    {
        public NhanVien NhanVien { get; set; }
        public ApplicationUser AspNetUser { get; set; }
        public string? UserName { get; set; }
    }
}
