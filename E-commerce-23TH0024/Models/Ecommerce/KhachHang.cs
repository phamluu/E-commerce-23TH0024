using E_commerce_23TH0024.Models.Identity;
using E_commerce_23TH0024.Models.Location;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_23TH0024.Models.Ecommerce;

public partial class KhachHang
{
    public int Id { get; set; }

    public string? HoTen { get; set; }

    public string? IdAspNetUsers { get; set; }

    public int? IdCustomerType { get; set; }

    public string? SoDienThoai { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public int? IdCity { get; set; }

    public int? IdDistrict { get; set; }

    public int? IdWard { get; set; }

    public string? DiaChi { get; set; }

    //public virtual City? City { get; set; }

    public virtual CustomerType? CustomerType { get; set; }

    //public virtual District? District { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    [NotMapped]
    public virtual ApplicationUser? AspNetUser { get; set; }
    //public virtual Ward? Ward { get; set; }
}
