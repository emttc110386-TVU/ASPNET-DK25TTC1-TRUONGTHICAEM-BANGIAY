using Microsoft.AspNetCore.Mvc;
using BangiayCAEM.Models;
using System.Text.Json;

namespace BangiayCAEM.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<CartItem> GetCart()
        {
            var sessionData = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(sessionData)) return new List<CartItem>();
            return JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            ViewBag.Total = cart.Sum(item => item.Product.Price * item.Quantity);
            return View(cart);
        }

        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                var cart = GetCart();
                var item = cart.FirstOrDefault(c => c.Product.Id == id);
                if (item != null)
                {
                    item.Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { Product = product, Quantity = 1 });
                }
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            cart.RemoveAll(c => c.Product.Id == id);
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }
    }
}