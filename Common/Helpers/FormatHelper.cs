using System.Globalization;

namespace E_commerce_23TH0024.Common.Helpers
{
    public static class FormatHelper
    {
        public static string ToVnCurrency(this decimal? value)
        {
            if (!value.HasValue)
            {
                return "";
            }
            return string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", value);
        }
    }
}
