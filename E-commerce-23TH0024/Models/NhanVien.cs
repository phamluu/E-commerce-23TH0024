using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models;

public partial class NhanVien
{
    public int Id { get; set; }

    public string? HoTen { get; set; }

    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }

    public string? UserID { get; set; }

    public virtual ICollection<DonHang> DonHangMaNvduyetNavigations { get; set; } = new List<DonHang>();

    public virtual ICollection<DonHang> DonHangMaNvghNavigations { get; set; } = new List<DonHang>();

    public virtual AspNetUsers? AspNetUser { get; set; }
}
