using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models.Identity;

public partial class AspNetRole
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<AspNetUsers> Users { get; set; } = new List<AspNetUsers>();
}
