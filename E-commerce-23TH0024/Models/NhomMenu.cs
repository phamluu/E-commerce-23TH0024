using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models;

public partial class NhomMenu
{
    public string Id { get; set; } = null!;

    public string? TenNhomMenu { get; set; }

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();
}
