
using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models.Ecommerce;

public partial class ChiTietDonHang
{
    public int Id { get; set; }

    public int IdDonHang { get; set; }

    public int IdSanPham { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public decimal DiscountApplied { get; set; }

    public virtual SanPham SanPham { get; set; } = null!;

    public virtual DonHang DonHang { get; set; } = null!;
}
