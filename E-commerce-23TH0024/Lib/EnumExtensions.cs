using E_commerce_23TH0024.Lib.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace E_commerce_23TH0024.Lib
{
    public static class EnumExtensions
    {
       
        public static string GetDisplayName(this Enum enumValue)
        {
            if (enumValue == null) return "";
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null) return enumValue.ToString();

            var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? enumValue.ToString();
        }

        public static string GetOrderStatusName(int? status)
        {
            if (!status.HasValue) return "";
            if (!Enum.IsDefined(typeof(OrderStatus), status.Value))
                return "Không xác định";

            return ((OrderStatus)status.Value).GetDisplayName();
        }
    }
}
