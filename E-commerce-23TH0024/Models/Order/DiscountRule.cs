namespace E_commerce_23TH0024.Models.Order;

public partial class DiscountRule
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Discount_Type { get; set; }

    public string? Description { get; set; }

    public decimal MinTotalPrice { get; set; }

    public decimal DiscountAmount { get; set; }

    public int DiscountPercent { get; set; }

    public int? IdLoaiSanPham { get; set; }

    public int? IdCustomerType { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? Created_at { get; set; }

    public virtual CustomerType? CustomerType { get; set; }
}
