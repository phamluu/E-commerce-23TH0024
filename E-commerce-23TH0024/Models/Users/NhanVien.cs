using E_commerce_23TH0024.Models.Identity;
using E_commerce_23TH0024.Models.Ecommerce;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_23TH0024.Models.Users;

public partial class NhanVien
{
    public int Id { get; set; }

    public string? HoTen { get; set; }

    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }

    public string? IdAspNetUsers { get; set; }
    //[InverseProperty("NhanVienGiao")]
    //public virtual ICollection<DonHang> DonHangNhanVienGiao { get; set; } = new List<DonHang>();
    //[InverseProperty("NhanVienDuyet")]
    //public virtual ICollection<DonHang> DonHangNhanVienDuyet { get; set; } = new List<DonHang>();
    
    public virtual ApplicationUser? AspNetUser { get; set; }
}
