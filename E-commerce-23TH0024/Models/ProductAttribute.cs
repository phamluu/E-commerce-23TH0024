using System;
using System.Collections.Generic;

namespace E_commerce_23TH0024.Models;

public partial class ProductAttribute
{
    public string Id { get; set; } = null!;

    public string? AttributeName { get; set; }

    public virtual ICollection<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();

    public virtual ICollection<ProductVariantAttribute> ProductVariantAttributes { get; set; } = new List<ProductVariantAttribute>();
}
