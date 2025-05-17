using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_23TH0024.Models.Ecommerce;

public partial class ProductVariant
{
    [Key]
    public int Id { get; set; }

    public int? IdSanPham { get; set; }

    public int? StockQuantity { get; set; }

    public decimal? Price { get; set; }
    [ForeignKey("IdSanPham")]
    public virtual SanPham? SanPham { get; set; }

    public virtual ICollection<ProductVariantAttribute> ProductVariantAttributes { get; set; } = new List<ProductVariantAttribute>();
}
