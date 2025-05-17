using E_commerce_23TH0024.Models.Location;
using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models.Ecommerce;

public partial class ShippingRate
{
    public int Id { get; set; }

    public int? IdDeliveryMethod { get; set; }

    public double? FromDistance { get; set; }

    public double? ToDistance { get; set; }

    public decimal FixedPrice { get; set; }

    public decimal PricePerKm { get; set; }

    public int? IdCity { get; set; }

    public decimal? WeightLitmit { get; set; }

    public DateTime? CreateAt { get; set; }

    //public virtual City? City { get; set; }

    public virtual DeliveryMethod? DeliveryMethod { get; set; }
}
