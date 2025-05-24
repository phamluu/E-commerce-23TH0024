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
        public LoaiSanPham? LoaiSanPham { get; set; }
        public string MenuName
        {
            get
            {
                switch (LoaiMenu)
                {
                    case (int?)EnumLoaiMenu.LoaiSanPham:
                        return LoaiSanPham != null ? this.LoaiSanPham.TenLSP : string.Empty;
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
                        return LoaiSanPham != null ? Helper.RemoveVietnameseAccent(LoaiSanPham?.TenLSP) + "-" + LoaiSanPham.Id : string.Empty;
                    default:
                        return string.Empty;
                }
            }
        }
    }
}