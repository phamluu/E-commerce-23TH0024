using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_23TH0024.Models;

public partial class ProductVariantAttribute
{
    public int Id { get; set; }

    public string? IdProductAttribute { get; set; }

    public int? IdProductVariant { get; set; }

    public int? IdAttributeValue { get; set; }

    public virtual ProductAttribute? ProductAttribute { get; set; }

    public virtual AttributeValue? AttributeValue { get; set; }
    [ForeignKey("IdProductVariant")]
    public virtual ProductVariant? ProductVariant { get; set; }
}
