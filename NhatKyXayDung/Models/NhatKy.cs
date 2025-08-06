using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NhatKyXayDung.Models
{
    public class NhatKy
    {
        public int Id { get; set; }
        public string? TenNhatKy { get; set; }
        public string? NoiDung { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime? NgayTrienKhai { get; set; }
        public int? STT { get; set; }
        public int IdCongTrinh { get; set; }
        public CongTrinh CongTrinh { get; set; }
        public int? TrangThai { get; set; }
        public int? ThoiTietSang { get; set; }
        public int? ThoiTietChieu { get; set; }
    }
}
