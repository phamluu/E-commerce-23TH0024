using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Models.Order;
using E_commerce_23TH0024.Models.Users;

namespace E_commerce_23TH0024.ViewModels
{
    public class DonHangViewModel : DonHang
    {
        public List<DonHangDetailViewModel> ChiTietDonHangVM { get; set; }
        public decimal? TotalProductAmount { get; set; }
        public NhanVien? NhanVienGiao { get; set; }
        public NhanVien? NhanVienDuyet { get; set; }
    }

    public class DonHangDetailViewModel:ChiTietDonHang
    {
        public SanPham SanPham { get; set; }
    }
}
