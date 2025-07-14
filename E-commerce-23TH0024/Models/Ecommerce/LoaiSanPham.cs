namespace E_commerce_23TH0024.Models.Ecommerce;

public partial class LoaiSanPham
{
    public int Id { get; set; }
    
    public string? TenLSP { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
