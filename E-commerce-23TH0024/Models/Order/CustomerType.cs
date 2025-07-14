using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models.Order;

public partial class CustomerType
{
    public int Id { get; set; }

    public string? CustomerTypeName { get; set; }

    public virtual ICollection<DiscountRule> DiscountRules { get; set; } = new List<DiscountRule>();

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();
}
