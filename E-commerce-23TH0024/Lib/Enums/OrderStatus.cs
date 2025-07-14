using System.ComponentModel.DataAnnotations;

namespace E_commerce_23TH0024.Lib.Enums
{
    public enum PaymentStatus
    {
        [Display(Name = "Đang chờ xử lý")]
        Pending = 1,

        [Display(Name = "Đã hoàn tất")]
        Completed = 2,

        [Display(Name = "Thất bại")]
        Failed = 3,

        [Display(Name = "Đã hủy")]
        Cancelled = 4,

        [Display(Name = "Đã hoàn tiền")]
        Refunded = 5
    }
    public enum OrderStatus
    {
        [Display(Name = "Chờ xử lý")]
        Pending = 1,

        [Display(Name = "Đã xác nhận")]
        Confirmed = 2,

        [Display(Name = "Đang xử lý")]
        Processing = 3,

        [Display(Name = "Đang giao hàng")]
        Shipped = 4,

        [Display(Name = "Đã giao hàng")]
        Delivered = 5,

        [Display(Name = "Đã hủy")]
        Cancelled = 6,

        [Display(Name = "Đã hoàn trả")]
        Returned = 7,

        [Display(Name = "Thất bại")]
        Failed = 8,

        [Display(Name = "Đã hoàn tiền")]
        Refunded = 9
    }
}
