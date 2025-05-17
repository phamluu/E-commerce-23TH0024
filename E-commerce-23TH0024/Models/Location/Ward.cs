using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models.Location;

public partial class Ward
{
    public int Id { get; set; }

    public string? WardName { get; set; }

    public int? IdDistrict { get; set; }

    public virtual District? District { get; set; }

    //public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();
}
