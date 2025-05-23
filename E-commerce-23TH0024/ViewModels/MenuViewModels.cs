using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Models.SystemSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_commerce_23TH0024.Models
{
    public class MenuViewModels:Menu
    {
        public LoaiSanPham LoaiSanPham { get; set; }
        public string MenuName
        {
            get
            {
                switch (LoaiMenu)
                {
                    case (int?)EnumLoaiMenu.LoaiSanPham:
                        return this.LoaiSanPham.TenLSP;
                    default:
                        return string.Empty;
                }
            }
        }
        public string Url
        {
            get
            {
                switch (LoaiMenu)
                {
                    case (int?)EnumLoaiMenu.LoaiSanPham:
                        return Helper.RemoveVietnameseAccent(LoaiSanPham.TenLSP) + "-" + LoaiSanPham.Id;
                    default:
                        return string.Empty;
                }
            }
        }
    }
}