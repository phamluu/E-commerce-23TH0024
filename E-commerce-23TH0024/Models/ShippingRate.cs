using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models;

public partial class ShippingRate
{
    public int Id { get; set; }

    public int? ShippingMethodID { get; set; }

    public double? FromDistance { get; set; }

    public double? ToDistance { get; set; }

    public decimal FixedPrice { get; set; }

    public decimal PricePerKm { get; set; }

    public int? Region { get; set; }

    public decimal? WeightLitmit { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual City? RegionNavigation { get; set; }

    public virtual DeliveryMethod? DeliveryMethod { get; set; }
}
