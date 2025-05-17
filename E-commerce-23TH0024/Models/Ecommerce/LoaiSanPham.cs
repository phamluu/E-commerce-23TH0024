using E_commerce_23TH0024.Models.SystemSetting;
using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models.Ecommerce;

public partial class LoaiSanPham
{
    public int Id { get; set; }

    public string? TenLSP { get; set; }

    public virtual ICollection<DiscountRule> DiscountRules { get; set; } = new List<DiscountRule>();

    //public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
