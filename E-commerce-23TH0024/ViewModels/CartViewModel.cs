using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Models.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace E_commerce_23TH0024.Models
{
    public class DonHangViewModel:DonHang
    {
        public decimal? TotalProductAmount { get; set; }
        public NhanVien NhanVienGiao { get; set; }
        public NhanVien NhanVienDuyet { get; set; }
    }
    public class CartItem:SanPhamViewModels 
    {
        //public int MaSP { get; set; }
        //public decimal? DonGia { get; set; }
        //public string TenSP { get; set; }
        //public string Anh { get; set; }
        public int SoLuong { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Total
        {
            get
            {
                if (Discount != null)
                {
                    return FinalPrice * SoLuong;
                }
                return DonGia * SoLuong;
            }
        }
        public decimal? WeightTotal => this.SoLuong * this.Weight;
    }

    public class Cart 
    {
        public decimal? Total => Items.Sum(x => x.Total);
        public decimal? WeightTotal => Items.Sum(x => x.WeightTotal);
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.SoLuong += item.SoLuong;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.Id == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public void UpdateItemQuantity(int productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.Id == productId);
            if (item != null)
            {
                item.SoLuong = quantity;
            }
        }

        public void Clear()
        {
            Items.Clear();
        }
    }

    public class ShippingViewModel
    {
      public  string addressFrom { get; set; }
      public  string addressTo { get; set; }
      public decimal weight { get; set; }
      public int shippingMethod {  get; set; }
      public decimal? shippingFee { get; set; }
      public double Distance { get; set; }
       public ShippingRate ShippingRate { get; set; }
    }
}