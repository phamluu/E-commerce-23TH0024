using E_commerce_23TH0024.Models.Ecommerce;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.Data.Seed
{
    public class SanPhamSeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var products = new List<SanPham>();

            for (int i = 6; i <= 16; i++)
            {
                string tenSP = "HL0" + i.ToString();
                string anh = "HL" + i.ToString() + ".jpg";

                products.Add(new SanPham
                {
                    Id = i, 
                    IdLoaiSanPham = 1,
                    TenSP = tenSP,
                    Anh = anh,
                    DonGia = 100000 + i * 1000,
                    DVT = "Cái",
                    MoTa = "Mô tả sản phẩm " + tenSP
                });
            }

            modelBuilder.Entity<SanPham>().HasData(products.ToArray());
        }
    }
}
