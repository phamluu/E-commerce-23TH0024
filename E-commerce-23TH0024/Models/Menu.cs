using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models;

public partial class Menu
{
    public int Id { get; set; }

    public string? NhomMenu { get; set; }

    public int? LoaiMenu { get; set; }

    public int? MaLoaiMenu { get; set; }

    public virtual LoaiSanPham? LoaiSanPham { get; set; }

    public virtual NhomMenu? NhomMenu1 { get; set; }
}
