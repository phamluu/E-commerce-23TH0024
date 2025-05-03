using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models;

public partial class KhachHang
{
    public int Id { get; set; }

    public string? HoTen { get; set; }

    public string? UserID { get; set; }

    public int? CustomerTypeID { get; set; }

    public string? SoDienThoai { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public int? CityID { get; set; }

    public int? DistrictId { get; set; }

    public int? WardId { get; set; }

    public string? DiaChi { get; set; }

    public virtual City? City { get; set; }

    public virtual CustomerType? CustomerType { get; set; }

    public virtual District? District { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual AspNetUsers? AspNetUser { get; set; }

    public virtual Ward? Ward { get; set; }
}
