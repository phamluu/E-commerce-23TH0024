using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_commerce_23TH0024.Models
{
    public class LoaiSanPhamViewModels : LoaiSanPham
    {
        public string Url
        {
            get
            {
                return Helper.RemoveVietnameseAccent(this.TenLSP) + "-" + this.Id;
            }
        }
    }
    public class SanPhamViewModels:SanPham
    {
        //private IEnumerable<DiscountRule> _validDiscountRulesCache;
        //private IEnumerable<DiscountRule> ValidDiscountRules
        //{
        //    get
        //    {
        //        if (_validDiscountRulesCache == null)
        //        {
        //            if (LoaiSanPham != null)
        //            {
        //                _validDiscountRulesCache = LoaiSanPham.DiscountRules
        //                .Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now);
        //            }
        //        }
        //        return _validDiscountRulesCache;
        //    }
        //}
        //public decimal DiscountMax()
        //    {
        //    if (DonGia.HasValue)
        //    {
        //        var maxDiscount = ValidDiscountRules
        //            .Max(x => DonGia.Value * x.DiscountPercent / 100 + x.DiscountAmount);
        //        return maxDiscount;
        //    }
        //    return 0; 
        //   } 
        //public DiscountRule Discount
        //{
        //    get
        //    {
               
        //        if (ValidDiscountRules != null && ValidDiscountRules.Any() && DonGia.HasValue)
        //        {
        //            var maxDiscount = DiscountMax();
        //            var discount = ValidDiscountRules
        //                    .Where(x => (DonGia.Value * x.DiscountPercent / 100 + x.DiscountAmount) == maxDiscount)
        //                    .FirstOrDefault();
        //            return discount;
        //        }
        //        return null;
        //    }
        //}
        public DiscountRule Discount { get; set; }
        public decimal? FinalPrice
        {
            get
            {
                //if (Discount != null)
                //{
                //    return DonGia.Value - DiscountMax();
                //}
                return DonGia;
            }
        }
        public string Url
        {
            get
            {
                if (LoaiSanPham != null)
                {
                    if (!string.IsNullOrEmpty(this.LoaiSanPham.TenLSP))
                    {
                        return Helper.RemoveVietnameseAccent(this.LoaiSanPham.TenLSP) + "/" + Helper.RemoveVietnameseAccent(this.TenSP) + "-" + this.Id;
                    }
                }
                return Helper.RemoveVietnameseAccent(this.TenSP) + "-" + this.Id;
            }
        }
    }
}