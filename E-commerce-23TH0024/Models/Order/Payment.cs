namespace E_commerce_23TH0024.Models.Order
{
    public class Payment
    {
        public int Id { get; set; }
        public int IdDonHang { get; set; }
        public decimal Amount { get; set; }
        public int PaymentStatus { get; set; }
        public int PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? GatewayTransactionId { get; set; }
        public virtual DonHang DonHang { get; set; }
    }
}
