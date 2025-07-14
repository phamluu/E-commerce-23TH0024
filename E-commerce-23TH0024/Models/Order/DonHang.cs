namespace E_commerce_23TH0024.Models.Order;

public partial class DonHang
{
    public int Id { get; set; }

    public DateTime? NgayDatHang { get; set; }

    public DateTime? NgayGiaoHang { get; set; }

    public int? IdKhachHang { get; set; }

    public int? IdNhanVienGiao { get; set; }

    public int? IdNhanVienDuyet { get; set; }

    public int? TinhTrang { get; set; } // 0: chua duyet, 1: da duyet
    public decimal VAT { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal ShippingFee { get; set; }

    public int? IdDeliveryMethod { get; set; }

    public decimal TotalProductAmount { get; set; }

    public int? IdDiscountRule { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual DiscountRule? DiscountRule { get; set; }

    public virtual KhachHang? KhachHang { get; set; }

    //[ForeignKey("IdNhanVienGiao")]
    //public virtual NhanVien? NhanVienGiao { get; set; }

    //[ForeignKey("IdNhanVienDuyet")]
    //public virtual NhanVien? NhanVienDuyet { get; set; }

    public virtual DeliveryMethod? DeliveryMethod { get; set; }
}
