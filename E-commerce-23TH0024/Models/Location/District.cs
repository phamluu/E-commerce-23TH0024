using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models.Location;

public partial class District
{
    public int Id { get; set; }

    public string? DistrictName { get; set; }

    public int? IdCity { get; set; }

    public virtual City? City { get; set; }

    //public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();

    public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
}
