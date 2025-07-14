using E_commerce_23TH0024.Models.Ecommerce;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.Data.Seed
{
    public class SanPhamSeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Seed LoaiSanPham
            modelBuilder.Entity<LoaiSanPham>().HasData(
                new LoaiSanPham { Id = 1, TenLSP = "Thiệp cưới cao cấp" },
                new LoaiSanPham { Id = 2, TenLSP = "Thiệp cưới giá rẻ" }
            );

            // Seed SanPham
            modelBuilder.Entity<SanPham>().HasData(
                new SanPham
                {
                    Id = 1,
                    IdLoaiSanPham = 1,
                    TenSP = "HL01",
                    Anh = "HL01.jpg",
                    DonGia = 110000,
                    DVT = "Cái",
                    MoTa = "Thiệp cưới HL01"
                }
            );
        }
    }
}
