using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_23TH0024.Models;

[Table("SanPham")]
public partial class SanPham
{
    public int Id { get; set; }

    public int? MaLSP { get; set; }

    public string? TenSP { get; set; }

    public string? MoTa { get; set; }

    public decimal? DonGia { get; set; }

    public string? DVT { get; set; }

    public string? Anh { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    [ForeignKey("MaLSP")]
    public virtual LoaiSanPham? LoaiSanPham { get; set; }

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
