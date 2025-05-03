using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models;

public partial class DeliveryMethod
{
    public int Id { get; set; }

    public string? MethodName { get; set; }

    public string? Description { get; set; }

    public decimal? Cost { get; set; }

    public string? DeliveryTime { get; set; }

    public bool ActiveStatus { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<ShippingRate> ShippingRates { get; set; } = new List<ShippingRate>();
}
