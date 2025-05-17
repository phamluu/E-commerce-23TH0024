using E_commerce_23TH0024.Models.Ecommerce;
using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models.SystemSetting;

public partial class Menu
{
    public int Id { get; set; }

    public string? IdNhomMenu { get; set; }

    public int? LoaiMenu { get; set; }

    public int? MaLoaiMenu { get; set; }
    public NhomMenu? NhomMenu { get; set; }
}
