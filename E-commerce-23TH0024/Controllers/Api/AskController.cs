using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace E_commerce_23TH0024.Controllers.Api
{
    public class AskController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Ask()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var json = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body);

            if (!json.TryGetValue("keywords", out var keywordElement) || keywordElement.ValueKind != JsonValueKind.Array)
            {
                return BadRequest("Thiếu 'keywords' hoặc không hợp lệ.");
            }

            var keywords = keywordElement.EnumerateArray().Select(k => k.GetString()?.ToLower()).Where(k => !string.IsNullOrEmpty(k)).ToList();

            if (keywords.Count == 0)
                return BadRequest("Không có từ khóa hợp lệ.");

            var query = _context.SanPham
                .Where(sp =>
                    keywords.Any(k =>
                        (sp.TenSP + " " + sp.MoTa).ToLower().Contains(k)))
                .Select(sp => new { sp.TenSP, sp.MoTa })
                .ToList();

            var results = query.Select(item => $"- **{item.TenSP}**: {item.MoTa}").ToList();

            if (results.Count == 0)
                return Ok("Không tìm thấy nội dung liên quan.");

            var markdown = string.Join("\n", results);
            return Ok(markdown);
        }

    }
}
