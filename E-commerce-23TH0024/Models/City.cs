using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models;

public partial class City
{
    public int Id { get; set; }

    public string? CityName { get; set; }

    public virtual ICollection<District> Districts { get; set; } = new List<District>();

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();

    public virtual ICollection<ShippingRate> ShippingRates { get; set; } = new List<ShippingRate>();
}
