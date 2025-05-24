using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_23TH0024.Controllers
{
    public class Cart_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHttpContextAccessor _contextAccessor;
        public Cart_23TH0024Controller(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            db = context;
            _contextAccessor = contextAccessor;
        }
        public ActionResult Index()
        {
            Cart cart = GetCart();
            foreach (var item in cart.Items) { 
                var product = db.SanPham.SingleOrDefault(x => x.Id == item.Id);
                if (product != null)
                {
                    item.Anh = product.Anh;
                    item.TenSP = product.TenSP;
                    item.DonGia = product.DonGia.Value;
                    item.LoaiSanPham = product.LoaiSanPham;
                }
            }
            return View(cart);
        }
        private Cart GetCart()
        {
            var cartCookie = _contextAccessor.HttpContext?.Request.Cookies["Cart"];

            if (cartCookie == null)
            {
                return new Cart();
            }
                var cartData = cartCookie;
            try
            {
                var cart = JsonConvert.DeserializeObject<Cart>(cartData);
                if (cart == null)
                {
                    return new Cart();
                }
                return cart;
            }
            catch (JsonException ex)
            {
                return new Cart();
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddToCart(int productId, int quantity)
        {
            var product = db.SanPham.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                if (!product.DonGia.HasValue)
                {
                    return Json(new { success = false, responseText = "Sản phẩm chưa có đơn giá" });
                }
                var cartItem = new CartItem
                {
                    SoLuong = quantity,
                    Id = product.Id,
                    TenSP = product.TenSP,
                    DonGia = product.DonGia.Value,
                    Anh = product.Anh,
                };
                var cartCookie = _contextAccessor.HttpContext?.Request.Cookies["Cart"];
                Cart cart = new Cart();
                if (cartCookie != null)
                {
                    var cartData = cartCookie;
                    cart = JsonConvert.DeserializeObject<Cart>(cartData);
                }
                cart.AddItem(cartItem);
                SaveCart(cart);
                return Json(new { success = true, responseText = "Sản phẩm đã được thêm vào giỏ hàng" });
            }
            return Json(new { success = false, responseText = "Sản phẩm không tồn tại" });
        }
        private void SaveCart(Cart cart)
        {
            var cartData = JsonConvert.SerializeObject(cart);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddYears(1),
                HttpOnly = true,
                Secure = (bool)(_contextAccessor.HttpContext?.Request.IsHttps), 
                Path = "/"
            };
            _contextAccessor.HttpContext?.Response.Cookies.Append("Cart", cartData, cookieOptions);
        }
        public ActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            cart.RemoveItem(productId);
            SaveCart(cart);

            TempData["Message"] = "Sản phẩm đã được xóa khỏi giỏ hàng.";

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            cart.UpdateItemQuantity(productId, quantity);
            SaveCart(cart);
            return Json(new { success = true, responseText = "Cập nhật số lượng thành công" });
        }

        [HttpGet]
        public int ItemCartCount()
        {
            var cart = GetCart(); 
            return cart.Items.Count();
        }
        
    }
}