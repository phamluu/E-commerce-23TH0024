using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models;

public partial class DonHang
{
    public int Id { get; set; }

    public DateTime? NgayDatHang { get; set; }

    public DateTime? NgayGiaoHang { get; set; }

    public int? MaKH { get; set; }

    public int? MaNVDuyet { get; set; }

    public int? MaNVGH { get; set; }

    public int? TinhTrang { get; set; }

    public decimal VAT { get; set; }

    public decimal TotalAmount { get; set; }

    public int? PaymentMethod { get; set; }

    public decimal ShippingFee { get; set; }

    public int? ShippingMethodID { get; set; }

    public decimal TotalProductAmount { get; set; }

    public int? DiscountRuleId { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual DiscountRule? DiscountRule { get; set; }

    public virtual KhachHang? KhachHang { get; set; }

    //public virtual NhanVien? MaNVDuyetNavigation { get; set; }

    //public virtual NhanVien? MaNVGHNavigation { get; set; }

    public virtual DeliveryMethod? ShippingMethod { get; set; }
}
