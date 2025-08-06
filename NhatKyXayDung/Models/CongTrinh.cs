namespace NhatKyXayDung.Models
{
    public class CongTrinh
    {
        public int Id { get; set; }
        public string TenCongTrinh { get; set; }
        public string? DiaChi { get; set; }
        public DateTime? NgayTrienKhai { get; set; }
        public DateTime? NgayHoanThanh { get; set; }
        public string? MoTa { get; set; }
        public DateTime NgayTao { get; set; }
        public ICollection<NhatKy>? NhatKies { get; set; }
    }
}
