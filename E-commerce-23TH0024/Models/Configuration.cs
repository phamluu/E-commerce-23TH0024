using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models;

public partial class Configuration
{
    public int Id { get; set; }

    public string ConfigCode { get; set; } = null!;

    public string ConfigName { get; set; } = null!;

    public string? ConfigValue { get; set; }

    public int? ConfigType { get; set; }
}
