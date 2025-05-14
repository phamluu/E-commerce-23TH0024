using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Owin.BuilderProperties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    public class Shipping_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext _entity;
        private readonly IHttpContextAccessor _contextAccessor;
        private const string GoogleGeocodeApiKey = "AIzaSyALhO10CboVceYQvAGiSrsIinAaEnJ49aU";

        public Shipping_23TH0024Controller(IHttpContextAccessor contextAccessor = null)
        {
            _contextAccessor = contextAccessor;
        }

        // Lấy tạo độ
        public async Task<(double lat, double lng)> GetCoordinatesFromAddressAsync(string address)
        {
            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "InAn (phamthithanhluu1990@gmail.com)");
                var response = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<List<dynamic>>(response);
                if (result.Count > 0)
                {
                    double lat = result[0].lat;
                    double lng = result[0].lon;
                    return (lat, lng);
                }
                else
                {
                    throw new Exception("Không thể lấy tọa độ từ địa chỉ này.");
                }
            }
        }
        // Tính khoáng cách
        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371;
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        private static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        // Tính phí ship
        public async Task<(decimal, double)> CalculateShippingFee(string addressFrom, string addressTo,
             int? shippingMethod, double? weight = null)
        {
            var (lat1, lng1) = await GetCoordinatesFromAddressAsync(addressFrom);
            var (lat2, lng2) = await GetCoordinatesFromAddressAsync(addressTo);
            double distance = CalculateDistance(lat1, lng1, lat2, lng2);
            var fee = _entity.ShippingRates.FirstOrDefault(x =>
                    x.ShippingMethodID == shippingMethod
                    && x.FromDistance <= distance && x.ToDistance >= distance
                    );
            decimal ShippingFee = fee.FixedPrice + fee.PricePerKm * (decimal)distance;
            return (ShippingFee, distance);
        }
        public ShippingRate shipppingRate(int shippingMethod, double distance)
        {
            var fee = _entity.ShippingRates.FirstOrDefault(x =>
                    x.ShippingMethodID == shippingMethod
                    && x.FromDistance <= distance && x.ToDistance >= distance
                    );
            return fee;
        }
        // Tra cứu phí ship hàng
        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult ShippingFee()
        {
            ViewBag.shippingMethod = new SelectList(_entity.DeliveryMethods, "ShippingMethodID", "MethodName");
            return View();
        }
        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost]
        public async Task<ActionResult> ShippingFee(ShippingViewModel shipping)
        {
            if (_contextAccessor.HttpContext?.Request.HasFormContentType == true)
            {
                var form = _contextAccessor.HttpContext?.Request.Form;
                if (form != null && form["isFormSubmitted"] == "true")
                {
                    return View(shipping);
                }
            }
            ViewBag.shippingMethod = new SelectList(_entity.DeliveryMethods, "ShippingMethodID", "MethodName", shipping.shippingMethod);

            var (lat1, lng1) = await GetCoordinatesFromAddressAsync(shipping.addressFrom);
            var (lat2, lng2) = await GetCoordinatesFromAddressAsync(shipping.addressTo);
            double distance = CalculateDistance(lat1, lng1, lat2, lng2);
            shipping.Distance = distance;
            shipping.ShippingRate = shipppingRate(shipping.shippingMethod, distance);
            shipping.shippingFee = shipping.ShippingRate.FixedPrice + shipping.ShippingRate.PricePerKm * (decimal)distance;
            return View(shipping);
        }
    }
}