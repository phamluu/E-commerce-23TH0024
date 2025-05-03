using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_23TH0024.Models;

[Table("AttributeValue")]
public partial class AttributeValue
{
    public int Id { get; set; }

    public string? IdProductAttribute { get; set; }

    public string? Value { get; set; }

    [ForeignKey("IdProductAttribute")]

    public virtual ProductAttribute? ProductAttribute { get; set; }

    public virtual ICollection<ProductVariantAttribute> ProductVariantAttributes { get; set; } = new List<ProductVariantAttribute>();
}
