using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models;

public partial class AspNetUserLogin
{
    public string LoginProvider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual AspNetUsers User { get; set; } = null!;
}
