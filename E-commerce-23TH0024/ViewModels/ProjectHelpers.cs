using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Security.Claims;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace E_commerce_23TH0024.Models
{
    public enum EnumLoaiMenu
    {
        LoaiSanPham = 1,
        LoaiTinTuc = 2
    }
    
    public enum EnumLoaiCauHinh
    {
        Image = 1,
        Text = 2
    }
    public static class Helper
    {
        public static string money(decimal? money)
        {
            if (money == null) {
                return "";
            }
           return string.Format(new CultureInfo("vi-VN"), "{0:N0} ₫", money);
        }
        public static string RemoveVietnameseAccent(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            str = str.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            string result = sb.ToString().ToLower();
            result = Regex.Replace(result, @"\s+", "-");

            result = Regex.Replace(result, @"[^a-z0-9\-]", "");

            return result;
        }
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private const int TokenLength = 32; // Độ dài của token (số byte)

        public static async Task<string> GeneratePasswordResetTokenAsync()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[TokenLength];
                rng.GetBytes(tokenData);
                string token = BitConverter.ToString(tokenData).Replace("-", "").ToLower();
                return await Task.FromResult(token); 
            }
        }
    }


    public static class ObjectMapper
    {
        public static TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : new()
        {
            if (source == null)
                return default(TDestination);
            TDestination destination = new TDestination();
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDestination).GetProperties();
            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties
                    .FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);

                if (destinationProperty != null && sourceProperty.CanRead && destinationProperty.CanWrite)
                {
                    var value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
            }
            return destination;
        }
    }

}